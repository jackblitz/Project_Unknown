using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI element representing a Weapons state
/// </summary>
public class WeaponHUDItem : MonoBehaviour
{
    private Image mIcon;
    private Text mAmmoCount;

    private void Awake()
    {
        mIcon = GetComponentInChildren<Image>();
        mAmmoCount = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Sets up the UI view. 
    /// </summary>
    /// <param name="item"></param>
    public void OnSetState(WeaponItem item)
    {
        mIcon.sprite = item.UIView;
        OnUpdateClipState(item.ClipData);
    }

    /// <summary>
    /// Updates the clip state such as AmmoCount and Clip count
    /// </summary>
    /// <param name="clip"></param>
    public void OnUpdateClipState(Clip clip)
    {
        mAmmoCount.text = clip.BulletsLeft.ToString() + "/" + clip.ClipSize.ToString();
    }

    /// <summary>
    /// Removes self from UI stack
    /// </summary>
    public void OnRemoveItem()
    {
        Destroy(this.gameObject);
    }

}
