using Unity;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
    private WeaponItem mActiveWeapon;

    public Rig HandIKRig;
    public Transform WeaponParent;

    private void Start()
    {
        WeaponItem hasWeapon = GetComponentInChildren<WeaponItem>();

        if(hasWeapon != null)
        {
            OnEquipWeapon(hasWeapon);
        }
    }

    private void Update()
    {
        if(mActiveWeapon == null)
        {
            HandIKRig.weight = 0;
        }
    }

    public void OnPullTrigger()
    {
        if (mActiveWeapon != null)
        {
            mActiveWeapon.OnPullTrigger();
        }
    }

    public void OnReleaseTrigger()
    {
        if (mActiveWeapon != null)
        {
            mActiveWeapon.OnReleaseTrigger();
        }
    }

    public void OnReload()
    {
        if (mActiveWeapon != null)
        {
            mActiveWeapon.OnReload();
        }
    }

    public void OnSwitchActiveWeapon(int weapon)
    {

    }

    public void OnEquipWeapon(WeaponItem weaponItem)
    {
        mActiveWeapon = weaponItem;
        mActiveWeapon.transform.parent = WeaponParent;
        mActiveWeapon.transform.localPosition = Vector3.zero;
        mActiveWeapon.transform.localRotation = Quaternion.identity;

       HandIKRig.weight = 1;
    }
}

