using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class AutoAim : MonoBehaviour
{
    public float ViewRadius;

    [Range(0, 360)]
    public float ViewAngle;

    public LayerMask TargetMask;
    public LayerMask ObjectMask;
    public Transform Direction;

    private FieldOfView mFOV;
    public bool IsAiming = false;

    private List<FieldOfView.VisibleObject> mOrderedVisibleObjects = new List<FieldOfView.VisibleObject>();
    private VisibleObject mActiveTarget;


    // Start is called before the first frame update
    void Start()
    {
        mFOV = this.gameObject.AddComponent<FieldOfView>() as FieldOfView;
        mFOV.TargetMask = TargetMask;
        mFOV.ObjectMask = ObjectMask;
        mFOV.ViewAngle = ViewAngle;
        mFOV.ViewRadius = ViewRadius;
        mFOV.Direction = Direction;
    }

    // Update is called once per frame
    void Update()
    {
        mFOV.FindVisibleTargets();
    }

    public void OnSetAiming(bool isAiming)
    {
        IsAiming = isAiming;

        if (IsAiming)
        {
            mActiveTarget = OnCalculateActiveTarget();
        }
    }

    public VisibleObject OnCalculateActiveTarget()
    {
        if (mFOV.VisibleObjects.Count > 0 && IsAiming)
            return mFOV.VisibleObjects[0];

        return null;
    }

    /// <summary>
    /// Returns the current target locked on to
    /// </summary>
    /// <returns></returns>
    public Vector3 getActiveTargetDirection()
    {
        if(mActiveTarget != null)
            return mActiveTarget.Direction;

        return Vector3.zero;
    }
}
