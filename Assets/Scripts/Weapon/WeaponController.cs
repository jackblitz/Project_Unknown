using Unity;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponItem mActiveWeapon;

    public void OnPullTrigger()
    {
        mActiveWeapon.OnPullTrigger();
    }

    public void OnReleaseTrigger()
    {
        mActiveWeapon.OnReleaseTrigger();
    }

    public void OnReload()
    {
        mActiveWeapon.OnReload();
    }

    public void OnSwitchActiveWeapon(int weapon)
    {

    }
}

