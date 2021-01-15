using Unity;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
using System.Collections;
using UnityEngine.Events;

public class WeaponControllerEvent : UnityEvent<string, int, WeaponItem>
{
    public const string WEAPON_CHANGE = "WEAPON_CHANGE";
    public const string WEAPON_UPDATE = "WEAPON_ITEM_UPDATE";
    public enum WeaponEvent
    {
        Added = 0,
        Removed = 1
    }

    public enum WeaponItemEvents
    {
        Bullet_Count = 0,
        Reload = 1
    }

    public void OnWeaponAdded(WeaponItem weaponModel)
    {
        Invoke(WEAPON_CHANGE, (int)WeaponEvent.Added, weaponModel);
    }

    public void OnWeaponRemoved(WeaponItem weaponModel)
    {
        Invoke(WEAPON_CHANGE, (int)WeaponEvent.Removed, weaponModel);
    }

    public void OnBulletCountChanged(WeaponItem weaponModel)
    {
        Invoke(WEAPON_UPDATE, (int)WeaponItemEvents.Bullet_Count, weaponModel);
    }

    public void OnWeaponReloaded(WeaponItem weaponModel)
    {
        Invoke(WEAPON_UPDATE, (int)WeaponItemEvents.Reload, weaponModel);
    }

}
public class WeaponController : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }
    private WeaponItem[] EquippedWeapons = new WeaponItem[2];

    private int mActiveWeaponIndex = -1;

    public Rig HandIKRig;
    public Transform[] WeaponSlots;

    public Transform WeaponLeftGrip;
    public Transform WeaponRightGrip;
    public Transform LeftHandBone;
    public Animator RigController;

    public Transform WeaponTarget;

    public WeaponControllerEvent ControllerEvents = new WeaponControllerEvent();

    private void Start()
    {
        WeaponItem hasWeapon = GetComponentInChildren<WeaponItem>();

        if (hasWeapon != null)
        {
            OnEquipWeapon(hasWeapon);
        }
    }

    public WeaponItem getActiveWeapon()
    {
        if (mActiveWeaponIndex < 0 || mActiveWeaponIndex >= EquippedWeapons.Length)
        {
            return null;
        }
        return EquippedWeapons[mActiveWeaponIndex];
    }

    public WeaponItem GetWeapon(int index)
    {
        if(index < 0 || index >= EquippedWeapons.Length)
        {
            return null;
        }
        return EquippedWeapons[index];
    }

    private void Update()
    {
      /*  WeaponItem activeWeapon = getActiveWeapon();
        if (activeWeapon != null && !isHolstered)
        {
            activeWeapon.UpdateWeapon(Time.deltaTime);
        }*/
    }

    #region Active Weapons TriggerFire calls
    public void OnPullTrigger()
    {
        WeaponItem activeWeapon = getActiveWeapon();
        if (activeWeapon != null)
        {
            activeWeapon.OnPullTrigger();
        }
    }

    public void OnReleaseTrigger()
    {
        WeaponItem activeWeapon = getActiveWeapon();
        if (activeWeapon != null)
        {
            activeWeapon.OnReleaseTrigger();
        }
    }

#endregion
    public void OnReload()
    {
        //Animation in rig controller will notify the weapon of its reload state 
        WeaponItem activeWeapon = getActiveWeapon();
        if (activeWeapon != null)
        {
            if (!activeWeapon.isHolstered)
            {
                RigController.SetTrigger("reload_weapon");
                activeWeapon.IsReloading = true;
            }
        }
    }

    #region Weapon Switching
    /// <summary>
    /// Switches to the next weapon
    /// </summary>
    public void OnNextWeapon()
    {
        switch (mActiveWeaponIndex)
        {
            case (int)WeaponSlot.Primary:
                SetActiveWeapon(WeaponSlot.Secondary);
                break;
            case (int)WeaponSlot.Secondary:
                SetActiveWeapon(WeaponSlot.Primary);
                break;
        }
    }

    /// <summary>
    /// Switches to the previous weapon
    /// </summary>
    public void OnPreviousWeapon()
    {
        switch (mActiveWeaponIndex)
        {
            case (int)WeaponSlot.Primary:
                SetActiveWeapon(WeaponSlot.Secondary);
                break;
            case (int)WeaponSlot.Secondary:
                SetActiveWeapon(WeaponSlot.Primary);
                break;
        }
    }

    #endregion

    /// <summary>
    /// Called when the character has equiped a weapon
    /// </summary>
    /// <param name="weaponItem">Weapon Equiped</param>
    public void OnEquipWeapon(WeaponItem weaponItem)
    {
        int weaponSlotIndex = (int)weaponItem.WeaponSlot;
     
        //if we already have a secondary weapona or primary destory the current one
        var weapon = GetWeapon(weaponSlotIndex);

        if (weapon)
        {
            Destroy(weapon.gameObject);
        }

        weapon = weaponItem;
        weapon.RayCastDestination = WeaponTarget;
        weapon.WeaponRecoil.RigController = RigController;
        weapon.AnimationEvents = GetComponentInChildren<WeaponAimationEvent>();
        weapon.LeftHand = LeftHandBone;
        weapon.transform.SetParent(WeaponSlots[weaponSlotIndex], false);

        EquippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(weaponItem.WeaponSlot);

        ControllerEvents.OnWeaponAdded(weaponItem);
    }

    public void HolsterActiveWeapon()
    {
        bool isHolstered = RigController.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(mActiveWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(mActiveWeaponIndex));
        }
    }


    void SetActiveWeapon(WeaponSlot weaponSlotIndex)
    {
        int holsterIndex = mActiveWeaponIndex;
        int activateIndex = (int)weaponSlotIndex;
        
        if(holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }


    /// <summary>
    /// When weapon slots are we switchings between
    /// </summary>
    /// <param name="holsterIndex">Put away this slot</param>
    /// <param name="activeIndex">Get out this slot</param>
    /// <returns></returns>
    IEnumerator SwitchWeapon(int holsterIndex, int activeIndex)
    {
        mActiveWeaponIndex = activeIndex;
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
    }

    IEnumerator HolsterWeapon(int index)
    {
        var weapon = GetWeapon(index);
      
        if (weapon)
        {
            if (!weapon.isHolstered)
            {
                weapon.isHolstered = true;
                RigController.SetBool("holster_weapon", true);
                //TODO fix hack. This should keep looking into how much time is left in the animation
                yield return new WaitForSeconds(RigController.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);

        if (weapon)
        {
            // Play animation and then wait for it to finish
            RigController.SetBool("holster_weapon", false);
            RigController.Play("equip_" + weapon.WeaponName);
            yield return new WaitForSeconds(RigController.GetCurrentAnimatorStateInfo(0).length);
            /* do
             {
                 yield return new WaitForEndOfFrame();
             } while (RigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);*/
            weapon.isHolstered = false;
        }
    }
}

