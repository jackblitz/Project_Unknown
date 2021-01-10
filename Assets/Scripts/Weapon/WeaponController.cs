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
    Animator mCharAnim;
    AnimatorOverrideController mCharOverrideAnim;

    private void Start()
    {
        WeaponItem hasWeapon = GetComponentInChildren<WeaponItem>();

        mCharAnim = GetComponent<Animator>();
        mCharOverrideAnim = mCharAnim.runtimeAnimatorController as AnimatorOverrideController;

        if (hasWeapon != null)
        {
            OnEquipWeapon(hasWeapon);
        }
    }

    private void Update()
    {
        if(mActiveWeapon == null)
        {
            HandIKRig.weight = 0;
            mCharAnim.SetLayerWeight(1, 0.0f);
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
        if(mActiveWeapon != null)
        {
            Destroy(mActiveWeapon);
        }
        mActiveWeapon = weaponItem;
        mActiveWeapon.transform.parent = WeaponParent;
        mActiveWeapon.transform.localPosition = Vector3.zero;
        mActiveWeapon.transform.localRotation = Quaternion.identity;

       HandIKRig.weight = 1;

       mCharAnim.SetLayerWeight(1,1f);

        Invoke(nameof(SetAnimationDelayed), 0.001f);
    }

    void SetAnimationDelayed()
    {
        mCharOverrideAnim["weapon_anim_empty"] = mActiveWeapon.WeaponAnimator;
    }

    [ContextMenu("Save Weapon Pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(WeaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(WeaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(WeaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(mActiveWeapon.WeaponAnimator);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}

