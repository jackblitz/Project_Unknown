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

    /// <summary>
    /// Weapons Fire Type
    /// </summary>
    public WeaponType CurrentWeaponType { get; set; }

    /// <summary>
    /// Type of Bullet fire from the Weapon. Contains data on how the bullet interacts with the world
    /// </summary>
    public Bullet BulletType { get; set; }

    /// <summary>
    /// Data on the guns clip. Clip size and amount left
    /// </summary>
    public Clip ClipData { get; set; }

    /// <summary>
    /// Gun fire rate. How quickly for bullets fire
    /// </summary>
    public int FireRate = 25;

    /// <summary>
    /// List of the all the bullets currently in scene
    /// </summary>
    private List<Bullet> BulletsFire = new List<Bullet>();

    [Header("Weapon Fire Data")]
    public ParticleSystem MuzzelFlash;
    public ParticleSystem HitEffect;
    public Transform RayCastOrigin;

    [Header("IK Hand Positions")]
    Transform RightHandPosition;
    Transform LeftHandPosition;

    public Animator mWeaponAnimator;

    private Ray ray;
    private RaycastHit hitInfo;

    public void OnPullTrigger()
    {
        if (canFire())
        {
            FireBullet();
        }
    }

    /// <summary>
    /// Can the weapon currently fire a bullet
    /// </summary>
    /// <returns>Returns true if a bullet can be fired</returns>
    private bool canFire()
    {
        return true;
    }

    private void FireBullet()
    {
        MuzzelFlash.Emit(1);

        ray.origin = RayCastOrigin.position;
        ray.direction = RayCastOrigin.forward;

        if(Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.white, 1);
        }
    }

    public void OnReleaseTrigger()
    {

    }

    public void Reload()
    {

    }

    private void LateUpdate()
    {
        
    }
}

