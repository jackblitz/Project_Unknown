using Unity;
using UnityEngine;

public class Bullet
{
    public enum BulletType
    {
        Raycast = 0,
        Projectile = 1,
        Explosive = 2,
    }
    

    /// <summary>
    /// Speed of the bullet travels through world
    /// </summary>
    public float BulletSpeed = 1000f;

    /// <summary>
    /// The rate of which gravity is applied to bullet
    /// </summary>
    public float BulletDrop = 0.0f;

    /// <summary>
    /// Bullets tracer effect
    /// </summary>
    public TrailRenderer Tracer;

    public float Time;
    public Vector3 InitialPosition;
    public Vector3 InitialVelocity;

    public static Bullet CreateBullet(TrailRenderer tracerPrefab, Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.InitialPosition = position;
        bullet.InitialVelocity = velocity;
        bullet.Time = 0;
        //bullet.Tracer = Instantiate(tracerPrefab, ray.origin, Quaternion.identity);
        bullet.Tracer.AddPosition(position);
        return bullet;
    }

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * BulletDrop;
        return bullet.InitialPosition + (bullet.InitialVelocity * bullet.Time) + (0.5f * gravity * bullet.Time * bullet.Time);

    }
}

