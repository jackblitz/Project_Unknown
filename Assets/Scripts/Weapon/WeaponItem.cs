﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// WeaponItem: Contain information and data on the weapon
/// </summary>
public class WeaponItem : Item
{
    /// <summary>
    /// 
    /// </summary>
    public enum WeaponType
    {
        SingleFire = 0,
        Semi_Automatic = 1,
        Automatic = 2,
        Throw = 3
    }

    [Header("Weapon Bullet Data")]
    /// <summary>
    /// Data on the guns clip. Clip size and amount left
    /// </summary>
    [SerializeField] public Clip ClipData;

    /// <summary>
    /// Type of Bullet fire from the Weapon. Contains data on how the bullet interacts with the world
    /// </summary>
    [SerializeField] public Bullet BulletType;

    /// <summary>
    /// Weapons Fire Type
    /// </summary>
    [SerializeField] public WeaponType CurrentWeaponType;

    [Header("Weapon Specs")]
    /// <summary>
    /// Gun fire rate. How quickly for bullets fire
    /// </summary>
    public float FireRate = 0.5f;

    /// <summary>
    /// Guns damage to an object
    /// </summary>
    public float GunDamage = 1f;

    /// <summary>
    /// How hard does the gun hit the object
    /// </summary>
    public float HitForce = 100f;

    public WeaponController.WeaponSlot WeaponSlot;

    /// <summary>
    /// List of the all the bullets currently in scene
    /// </summary>
    private List<Bullet> BulletsFire = new List<Bullet>();

    [Header("Weapon Data")]
    public ParticleSystem MuzzelFlash;
    public ParticleSystem HitEffect;
    public Transform RayCastOrigin;
    public Transform RayCastDestination; // Exactly location to shoot at
    public TrailRenderer TracerEffect;
    public GameObject BulletVFX;
    public AudioSource GunAudio;

    [Header("Animation")]
    public string WeaponName;
    public WeaponRecoil WeaponRecoil;
    public WeaponAimationEvent AnimationEvents;
    //Reference to left hand location
    public Transform LeftHand;

    private Ray ray;
    private RaycastHit hitInfo;

    private float accumulatedTime = 0f;

    public bool hasFired { get; private set; }
    public bool IsPullingTrigger = false;
    public bool IsReloading = false;
    public bool isHolstered = true;

    private void Awake()
    {
        WeaponRecoil = GetComponent<WeaponRecoil>();
    }

    private void Start()
    {
        AnimationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

    /// <summary>
    /// All Animation events for Weapon come through here
    /// </summary>
    /// <param name="animationEvent">What are animating. Reloading, recoil, drop, pick up......</param>
    /// <param name="animationState">State of the animation event. Each animation event can have a state</param>
    private void OnAnimationEvent(string animationEvent, int animationState)
    {
        switch (animationEvent)
        {
            case WeaponAimationEvent.WEAPON_RELOAD:
                OnWeaponReloadEvents(animationState);
                break;
        }
    }

    /// <summary>
    /// All reload events from the animation controller roll through here
    /// </summary>
    /// <param name="state">Current State of the Animation</param>
    private void OnWeaponReloadEvents(int state)
    {
        if (isHolstered)
            return;
        switch (state)
        {
            case (int)WeaponAimationEvent.WeaponReloadEventState.Detach_Mag:
                OnDetachMagazine();
                break;
            case (int)WeaponAimationEvent.WeaponReloadEventState.Drop_Mag:
                OnDropMagazine();
                break;
            case (int)WeaponAimationEvent.WeaponReloadEventState.Refill_Mag:
                OnRefillMagazine();
                break;
            case (int)WeaponAimationEvent.WeaponReloadEventState.Attach_Mag:
                OnAttachMagazine();
                break;
            case (int)WeaponAimationEvent.WeaponReloadEventState.Complete:
                OnReload();
                break;
        }
    }

    /// <summary>
    /// Called via an animation event. Animation has notified us that the animation has attached the magazine
    /// </summary>
    private void OnAttachMagazine()
    {
        ClipData.Magazine.SetActive(true);
        Destroy(MagazineHand);
        WeaponRecoil.RigController.ResetTrigger("reload_weapon");
    }

    /// <summary>
    /// Called via an animation event. Animation has notified us that the animation in a refill state
    /// </summary>
    private void OnRefillMagazine()
    {
        MagazineHand.SetActive(true);
    }

    /// <summary>
    /// Called via an animation event. Animation has notified us that the animation is about to drop the magazine
    /// </summary>
    private void OnDropMagazine()
    {
        GameObject droppedMagazine = Instantiate(MagazineHand, MagazineHand.transform.position, MagazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        MagazineHand.SetActive(false);
    }

    /// <summary>
    /// Called via an animation event. Animation has notified us that the animation detaching the magizine
    /// </summary>
    private GameObject MagazineHand;
    private void OnDetachMagazine()
    {
        MagazineHand = Instantiate(ClipData.Magazine, LeftHand, true);
        ClipData.Magazine.SetActive(false);
    }


    public void OnPullTrigger()
    {
       IsPullingTrigger = true;
    }


    /// <summary>
    /// Can the weapon currently fire a bullet
    /// </summary>
    /// <returns>Returns true if a bullet can be fired</returns>
    private bool CanFire()
    {
        return Time.time > accumulatedTime;
    }

    /**
     * If our clip is empty we need to reload clip
     */
    private bool RequiresReload() {
       return (ClipData.BulletsLeft == 0) ? true : false;
    }

    /**
     * Does the player need to let go over the trigger to fire next round
     */
    private bool RequiresReset()
    {
        bool resetHammer = false;

        if (hasFired)
        {
            switch (CurrentWeaponType)
            {
                case WeaponType.SingleFire:
                    resetHammer = true;
                    break;
                case WeaponType.Semi_Automatic:
                    resetHammer = (ClipData.BurstCount >= ClipData.BurstRate) ? true : false;
                    break;
                case WeaponType.Automatic:
                    resetHammer = false;
                    break;
            }
        }

        return resetHammer;
    }

    private void FireBullet()
    {
        ClipData.BurstCount++;
        ClipData.BulletsLeft--;

        MuzzelFlash.Emit(1);

        // Play the shooting sound effect
        if(GunAudio != null)
            GunAudio.Play();

        Vector3 direction = (RayCastDestination.position - RayCastOrigin.position).normalized;

        var bullet = CreateBullet(RayCastOrigin.position, direction);
        BulletsFire.Add(bullet);

        WeaponRecoil.GenerateRecoil(WeaponName);
    }

    public void OnReleaseTrigger()
    {
        IsPullingTrigger = false;
        ClipData.BurstCount = 0 ;
    }

    public void OnReload()
    {
       ClipData.BurstCount = 0;
       ClipData.BulletsLeft = ClipData.ClipSize;
       IsReloading = false;
    }

    private void LateUpdate()
    {
        if (IsPullingTrigger && !isHolstered)
        {
            // Update the time when our player can fire next
            if (CanFire() && !RequiresReload() && !RequiresReset() && !IsReloading)
            {
                accumulatedTime = Time.time + FireRate;
                hasFired = true;
                FireBullet();
            }
        }
        else
        {
            hasFired = false;
        }

        SimmulateBullets(Time.deltaTime);
        DestroyBullets();
    }

   /* public void UpdateWeapon(float delta)
    {
        if (IsPullingTrigger)
        {
            // Update the time when our player can fire next
            if (CanFire() && !RequiresReload() && !RequiresReset())
            {
                accumulatedTime = Time.time + FireRate;
                hasFired = true;
                FireBullet();
            }
        }
        else
        {
            hasFired = false;
        }

        SimmulateBullets(delta);
        DestroyBullets();
    }*/
    private void DestroyBullets()
    {
        BulletsFire.ForEach(bullet =>
        {
            if(bullet.Time >= bullet.MaxLifeTime)
            {
                Destroy(bullet.Tracer);
            }
        });
        BulletsFire.RemoveAll(bullet => bullet.Time >= bullet.MaxLifeTime);
    }

    private void SimmulateBullets(float deltaTime)
    {
        foreach (var bullet in BulletsFire)
        {
            Vector3 p0 = bullet.GetPosition();
            bullet.Time += deltaTime;
            Vector3 p1 = bullet.GetPosition();
            RayCastSegment(p0, p1, bullet);
        }
    }

    private void RayCastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = (end - start);
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            HitEffect.transform.position = hitInfo.point;
            HitEffect.transform.forward = hitInfo.normal;
            HitEffect.Emit(1);

            bullet.Tracer.transform.position = hitInfo.point;
            bullet.Time = bullet.MaxLifeTime;
            
            if (bullet.BounceCount > 0)
            {
                bullet.Time = 0;
                bullet.InitialPosition = hitInfo.point;
                bullet.InitialVelocity = Vector3.Reflect(bullet.InitialVelocity, hitInfo.normal);
                bullet.BounceCount--;
            }


            // Check if the object we hit has a rigidbody attached
            var rigidBody = hitInfo.collider.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                // Add force to the rigidbody we hit, in the direction from which it was hit
                // hitInfo.rigidbody.AddForce(-hitInfo.normal * HitForce);
                rigidBody.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }
        }
        else
        {
            bullet.Tracer.transform.position = end;
        }
    }

    public Bullet CreateBullet(Vector3 position, Vector3 direction)
    {
        Bullet bullet = new Bullet(direction);
        bullet.InitialPosition = position;
        bullet.Time = 0;
        bullet.Tracer = Instantiate(BulletVFX, position, Quaternion.identity);
        bullet.Tracer.transform.forward = direction;
        bullet.BounceCount = bullet.MaxBounce;
       // bullet.Tracer.AddPosition(position);
        return bullet;
    }

}

