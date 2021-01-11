using System;
using Unity;
using UnityEngine;

[Serializable]
public class Bullet
{
    public enum BulletType
    {
        Projectile = 0,
        Explosive = 1,
    }
    

    /// <summary>
    /// Speed of the bullet travels through world
    /// </summary>
    public float BulletSpeed = 100;

    /// <summary>
    /// The rate of which gravity is applied to bullet
    /// </summary>
    public float BulletDrop = 0.0f;

    /// <summary>
    /// Max number of times the bullet can bounce
    /// </summary>
    public float MaxBounce = 0;

    public float BounceCount = 0;

    /// <summary>
    /// Bullets tracer effect
    /// </summary>
    // public TrailRenderer Tracer;
    public GameObject Tracer;

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

