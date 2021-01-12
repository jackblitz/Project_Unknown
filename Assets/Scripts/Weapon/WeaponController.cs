using Unity;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;
using System.Collections;

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
    public Animator RigController;

    public Transform WeaponTarget;
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

    public void OnReload()
    {
        WeaponItem activeWeapon = getActiveWeapon();
        if (activeWeapon != null)
        {
            activeWeapon.OnReload();
        }
    }

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
        weapon.transform.SetParent(WeaponSlots[weaponSlotIndex], false);

        EquippedWeapons[weaponSlotIndex] = weapon;

        SetActiveWeapon(weaponItem.WeaponSlot);
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
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
        mActiveWeaponIndex = activeIndex;
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

