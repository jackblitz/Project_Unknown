using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class AutoAim : FieldOfView
{
    public bool IsAiming = false;

    private VisibleObject mActiveTarget;

    // Update is called once per frame
    void Update()
    {
        FindVisibleTargets();
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
        if (VisibleObjects.Count > 0 && IsAiming)
            return VisibleObjects[0];

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

    public override void OnCompleteScan()
    {
    }
}
