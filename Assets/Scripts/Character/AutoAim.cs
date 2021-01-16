using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject mActiveTarget;


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
        
    }

    public void OnSetAiming(bool isAiming)
    {
        IsAiming = isAiming;

        if (IsAiming)
        {
            mActiveTarget = OnCalculateActiveTarget();
        }
    }

    public GameObject OnCalculateActiveTarget()
    {
        if (mFOV.VisibleObjects.Count > 0 && IsAiming)
            return mFOV.VisibleObjects[0].Object;

        return null;
    }

    /// <summary>
    /// Returns the current target locked on to
    /// </summary>
    /// <returns></returns>
    public GameObject getActiveTarget()
    {
        return mActiveTarget;
    }
}
