using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AILocomotion))]
[RequireComponent(typeof(Ragdoll))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(UIHit))]
public class AiController : MonoBehaviour
{
    private AILocomotion mAILocomotion;
    private Ragdoll mRagdoll;
    private Health mHealth;
    private Rigidbody[] mRigidbody;
    private UIHealthBar mHealthBar;
    private Hitbox[] mHitBoxs;
    private UIHit mUIHit;
    private Rigidbody mLastHitBodyPart;
    private Vector3 mLastHitLocation;
    private WeaponItem mWeaponHitWith;
    public float HitForce;

    // Start is called before the first frame update
    void Start()
    {
        mAILocomotion = GetComponent<AILocomotion>();
        mRagdoll = GetComponent<Ragdoll>();
        mRigidbody = GetComponentsInChildren<Rigidbody>();
        mHealthBar = GetComponentInChildren<UIHealthBar>();
        mUIHit = GetComponent<UIHit>();

        foreach (var rigidBody in mRigidbody)
        {
            rigidBody.gameObject.AddComponent<Hitbox>();
        }

        mHealth = GetComponent<Health>();

        mHealth.Event.AddListener(OnHealthEvents);

        mHitBoxs = GetComponentsInChildren<Hitbox>();

        foreach(Hitbox hitbox in mHitBoxs)
        {
            hitbox.Event.AddListener(OnHitBoxEvent);
        }
    }

    private void OnHealthEvents(int state)
    {
        switch (state)
        {
            case (int)HealthEvent.HealthEventState.TakenDamage:
                OnTakeDamage();
                break;
            case (int)HealthEvent.HealthEventState.Dead:
                OnDead();
                break;
        }
    }

    private void OnDead()
    {
        mRagdoll.OnActivateRagdoll();
       // mLastHitLocation.z = 1;

        //Hits the body part with force
        Vector3 force = mLastHitLocation * mWeaponHitWith.HitForce;
        mLastHitBodyPart.AddForce(force, ForceMode.VelocityChange);

        mHealthBar.gameObject.SetActive(false);
    }

    private void OnUpdateHealthUI()
    {
        mHealthBar.SetHealthBarPercent(mHealth.CurrentHealth / mHealth.MaxHealth);
    }

    private void OnTakeDamage()
    {
        mUIHit.OnRenderHit();
        OnUpdateHealthUI();
    }

    private void OnHitBoxEvent(WeaponItem weapon, Vector3 direction, Rigidbody hitBodyPart)
    {
        mHealth.OnTakeDemage(weapon.GunDamage);
        mLastHitBodyPart = hitBodyPart;
        mLastHitLocation = direction;
        mWeaponHitWith = weapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
