using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class AIFOV : MonoBehaviour
{
    public float ViewRadius;

    [Range(0, 360)]
    public float ViewAngle;

    public LayerMask TargetMask;
    public LayerMask ObjectMask;

    private FieldOfView mFOV;

    public Transform Direction;
    // Start is called before the first frame update
    void Start()
    {
        mFOV = this.gameObject.AddComponent<FieldOfView>() as FieldOfView;

        StartCoroutine("FindTargetsWithDelay", .5f);
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        mFOV.FindVisibleTargets();
    }

    public VisibleObject GetVisibleObject()
    {
        if (mFOV.VisibleObjects.Count > 0)
            return mFOV.VisibleObjects[0];

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        mFOV.TargetMask = TargetMask;
        mFOV.ObjectMask = ObjectMask;
        mFOV.ViewAngle = ViewAngle;
        mFOV.ViewRadius = ViewRadius;
        mFOV.Direction = Direction;
        //mFOV.FindVisibleTargets();
    }
}
