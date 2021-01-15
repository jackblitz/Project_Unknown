using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller used for updating and controlling the WeaponHUD
/// </summary>
public class WeaponHUDController : MonoBehaviour
{
    public WeaponController WeaponController;
    public GameObject WeaponItemPrefab;

    private Dictionary<WeaponController.WeaponSlot, WeaponHUDItem> WeaponSlots = new Dictionary<WeaponController.WeaponSlot, WeaponHUDItem>();

    private void Start()
    {
        WeaponController.ControllerEvents.AddListener(OnWeaponControllerEvents);
    }

    private void OnDestroy()
    {
        WeaponController.ControllerEvents.RemoveListener(OnWeaponControllerEvents);
    }

    /// <summary>
    /// Called when any event is called in the WeaponController. Used for informing the UI of changes to its state
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="eventState">State of the event</param>
    /// <param name="weapon">Weapon Item<param>
    private void OnWeaponControllerEvents(string eventName, int eventState, WeaponItem weapon)
    {
        switch (eventName)
        {
            case WeaponControllerEvent.WEAPON_CHANGE:
                OnWeaponChange(eventState, weapon);
                break;
            case WeaponControllerEvent.WEAPON_UPDATE:
                OnUpdateWeaponItemInfo(eventState, weapon);
                break;
        }
    }

    /// <summary>
    /// Called then the weaponcontroller weapon state has change. Used for informing the HUD of changes to its state
    /// </summary>
    /// <param name="eventState"></param>
    /// <param name="weapon"></param>
    private void OnWeaponChange(int eventState, WeaponItem weapon)
    {
        switch (eventState)
        {
            case (int)WeaponControllerEvent.WeaponEvent.Added:
                OnWeaponAdded(weapon);
                break;
            case (int)WeaponControllerEvent.WeaponEvent.Removed:
                OnWeaponRemoved(weapon);
                break;
        }
    }

    /// <summary>
    /// Called when the WeaponController has picked up a weapon
    /// </summary>
    /// <param name="model">Weapon which was picked up</param>
    private void OnWeaponAdded(WeaponItem model)
    {
        GameObject UIelement = Instantiate(WeaponItemPrefab, Vector3.zero, Quaternion.identity);
        UIelement.transform.SetParent(this.gameObject.transform, false);
        WeaponHUDItem HUDItem = UIelement.GetComponent<WeaponHUDItem>();
        HUDItem.OnSetState(model);

        WeaponSlots.Add(model.WeaponSlot, HUDItem);

        model.WeaponEvents.AddListener(OnWeaponControllerEvents);
    }

    /// <summary>
    /// Updates weapon information in HUD
    /// </summary>
    /// <param name="eventState"></param>
    /// <param name="weapon"></param>
    private void OnUpdateWeaponItemInfo(int eventState, WeaponItem weapon)
    {
        WeaponHUDItem item;
        
        WeaponSlots.TryGetValue(weapon.WeaponSlot, out item);

        if(item != null)
        {
            item.OnUpdateClipState(weapon.ClipData);
        }

    }


    /// <summary>
    /// Called when the WeaponController has dropped a weapon
    /// </summary>
    /// <param name="model">Weapon</param>
    private void OnWeaponRemoved(WeaponItem model)
    {
        model.WeaponEvents.AddListener(OnWeaponControllerEvents);
    }
}
