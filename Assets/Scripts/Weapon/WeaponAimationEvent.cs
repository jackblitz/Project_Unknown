using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : UnityEvent<string, int>
{

}
public class WeaponAimationEvent : MonoBehaviour
{
    public enum WeaponReloadEventState
    {
        Detach_Mag = 0,
        Drop_Mag = 1,
        Refill_Mag = 2,
        Attach_Mag = 3,
        Complete = 4
    }
    public const string WEAPON_RELOAD = "weapon_reload_event";

    public AnimationEvent WeaponAnimationEvent = new AnimationEvent();
    public void OnWeaponReloadEvent(int animationEvent)
    {
        WeaponAnimationEvent.Invoke(WEAPON_RELOAD, animationEvent);
    }
}
