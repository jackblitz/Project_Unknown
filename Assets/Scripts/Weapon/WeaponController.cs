using Unity;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
public class WeaponController : MonoBehaviour
{
    private WeaponItem mActiveWeapon;

    public Rig HandIKRig;
    public Transform WeaponParent;

    public Transform WeaponLeftGrip;
    public Transform WeaponRightGrip;
    public Animator RigController;

    private void Start()
    {
        WeaponItem hasWeapon = GetComponentInChildren<WeaponItem>();


        if (hasWeapon != null)
        {
            OnEquipWeapon(hasWeapon);
        }
    }

    private void Update()
    {
        if(mActiveWeapon == null)
        {
         
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

        OnHolsterWeapon();
    }

    public void OnSwitchActiveWeapon(int weapon)
    {

    }

    public void OnEquipWeapon(WeaponItem weaponItem)
    {
        if(mActiveWeapon != null)
        {
            Destroy(mActiveWeapon);
        }
        mActiveWeapon = weaponItem;
        mActiveWeapon.transform.parent = WeaponParent;
        mActiveWeapon.transform.localPosition = Vector3.zero;
        mActiveWeapon.transform.localRotation = Quaternion.identity;

        RigController.Play("equip_" + mActiveWeapon.WeaponName);
    }


    public void OnHolsterWeapon()
    {
        bool isHolstered = RigController.GetBool("holster_weapon");
        RigController.SetBool("holster_weapon", !isHolstered);
    }

}

