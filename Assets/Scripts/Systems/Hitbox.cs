using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [HideInInspector] public HitBoxEvent Event = new HitBoxEvent();
    public void OnRaycastHit(WeaponItem weapon, Vector3 direction)
    {
        Event.Invoke(weapon, direction);
    }
}

public class HitBoxEvent : UnityEvent<WeaponItem, Vector3>{

}
