using System;
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

    /// <summary>
    /// List of the all the bullets currently in scene
    /// </summary>
    private List<Bullet> BulletsFire = new List<Bullet>();

    [Header("Weapon Data")]
    public ParticleSystem MuzzelFlash;
    public ParticleSystem HitEffect;
    public Transform RayCastOrigin;
    public TrailRenderer TracerEffect;
    public GameObject BulletVFX;
    public AudioSource GunAudio;

    [Header("Animation")]
    public AnimationClip WeaponAnimator;

    private Ray ray;
    private RaycastHit hitInfo;

    private float accumulatedTime = 0f;

    public bool hasFired { get; private set; }
    public bool IsPullingTrigger { get; set; }

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

        Vector3 direction = RayCastOrigin.forward;

        var bullet = CreateBullet(RayCastOrigin.position, direction);
        BulletsFire.Add(bullet);
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
    }

    private void LateUpdate()
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

        SimmulateBullets(Time.deltaTime);
        DestroyBullets();
    }
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


            // Check if the object we hit has a rigidbody attached
            if (hitInfo.rigidbody != null)
            {
                // Add force to the rigidbody we hit, in the direction from which it was hit
                hitInfo.rigidbody.AddForce(-hitInfo.normal * HitForce);
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
        bullet.Tracer.transform.forward = direction.normalized;
       // bullet.Tracer.AddPosition(position);
        return bullet;
    }

}

