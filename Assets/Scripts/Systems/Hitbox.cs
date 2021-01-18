using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [HideInInspector] public HitBoxEvent Event = new HitBoxEvent();

    public void OnRaycastHit(WeaponItem weapon, Vector3 direction)
    {
        Rigidbody bodyPartHit = GetComponent<Rigidbody>();
        Event.Invoke(weapon, direction, bodyPartHit);
    }
}

public class HitBoxEvent : UnityEvent<WeaponItem, Vector3, Rigidbody>{

}
