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
    public float BulletSpeed = 20;

    /// <summary>
    /// The rate of which gravity is applied to bullet
    /// </summary>
    public float BulletDrop = 0.0f;

    /// <summary>
    /// Bullets tracer effect
    /// </summary>
    public TrailRenderer Tracer;

    public float Time;
    public float MaxLifeTime = 3f;
    public Vector3 InitialPosition;
    public Vector3 InitialVelocity;

    public Bullet(Vector3 direction)
    {
        InitialVelocity = direction * BulletSpeed;
    }

    public Vector3 GetPosition()
    {
        Vector3 gravity = Vector3.down * BulletDrop;
        return InitialPosition + (InitialVelocity * Time) + (0.5f * gravity * Time * Time);
    }

}

