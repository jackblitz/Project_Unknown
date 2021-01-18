using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float MaxTime = 1.0f;
    public float MaxDistance = 1.0f;
    public float ViewRadius = 5.0f;
    [Range(0, 360)]
    public float ViewAngle;

    public LayerMask TargetMask;
}
