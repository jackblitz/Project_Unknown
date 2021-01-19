using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static FieldOfView;

public class AIFOV : FieldOfView
{
    
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

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

    public VisibleObject GetClostestTarget()
    {
        if (VisibleObjects.Count > 0)
            return VisibleObjects[0];

        return null;
    }

    private int LastRange = -1;

    public override void OnCompleteScan()
    {
        if (LastRange != VisibleObjects.Count)
        {
            if (VisibleObjects.Count > 0)
            {
                Event.OnFoundFirstTarget(GetClostestTarget());
            }
            else
            {
                Event.OnLostTarget(null);
            }
        }

        LastRange = VisibleObjects.Count;
    }
}

