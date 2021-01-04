using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }
    public ParticleSystem MuzzelFlash;
    public ParticleSystem HitEffect;
    public Transform RayCastOrigin;
    public TrailRenderer TracerEffect;
    public bool isFiring = false;

    //25 bullets every 25
    public int FireRate = 25;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0.0f;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime = 0.0f;

    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3;

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);

    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0;
        bullet.tracer = Instantiate(TracerEffect, ray.origin, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1 / FireRate;
       // while(accumulatedTime >= 0.0f)
       // {
//FireBullet();
       //     accumulatedTime -= fireInterval;
       // }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RayCastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
       // bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
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

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }

    public void StopFiriing()
    {
        isFiring = false;
    }

    private void FireBullet()
    {
        MuzzelFlash.Emit(1);

        var bullet = CreateBullet(RayCastOrigin.position, RayCastOrigin.forward);

        bullets.Add(bullet);
    }
}
