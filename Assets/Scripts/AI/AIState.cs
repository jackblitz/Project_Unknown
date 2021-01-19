using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public enum AIStateId
{
    FollowTarget,
    Patrol,
    Death,
    Idle,
    Search
}
public interface AIState
{
    AIStateId GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);

    void OnFOVEvent(AIAgent agent, int state, VisibleObject visibleObject);
}
