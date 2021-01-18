using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;

    public HealthEvent Event = new HealthEvent();

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTakeDemage(float amount)
    {
        CurrentHealth -= amount;

        Event.OnTakingDamage();

        if (CurrentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Event.OnDead();
    }
}

public class HealthEvent : UnityEvent<int>
{
    public enum HealthEventState
    {
        TakenDamage = 0,
        Dead = 1
    }
    public void OnDead()
    {
        Invoke((int)HealthEventState.Dead);
    }

    public void OnTakingDamage()
    {
        Invoke((int)HealthEventState.TakenDamage);
    }

}
