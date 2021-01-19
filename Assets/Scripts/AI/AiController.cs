using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AILocomotion))]
[RequireComponent(typeof(Ragdoll))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(UIHit))]
[RequireComponent(typeof(AIAgent))]
public class AiController : MonoBehaviour
{
    private AILocomotion mAILocomotion;
    private Ragdoll mRagdoll;
    private Health mHealth;
    private Rigidbody[] mRigidbody;
    private UIHealthBar mHealthBar;
    private Hitbox[] mHitBoxs;
    private UIHit mUIHit;
    private BoxCollider mBoxCollider;

    private AIAgent AIAgent;

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
        mBoxCollider = GetComponent<BoxCollider>();
        AIAgent = GetComponent<AIAgent>();

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
        //Hits the body part with force
        AIDeathState deathState = AIAgent.StateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = mLastHitLocation;
        deathState.KillBy = mWeaponHitWith;


        AIAgent.StateMachine.OnChangeState(AIStateId.Death);
        //Switch collider off so that auto aim stops
        mBoxCollider.enabled = false;
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
        mRagdoll.SetOnLastHitBody(hitBodyPart);
      
        mLastHitLocation = direction;
        mWeaponHitWith = weapon;

        AISearchState searchState = AIAgent.StateMachine.GetState(AIStateId.Search) as AISearchState;
        searchState.LastKnownLocation = mWeaponHitWith.gameObject.transform.position;
        AIAgent.StateMachine.OnChangeState(AIStateId.Search);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
