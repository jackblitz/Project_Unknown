using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class AIIdleState : AIState
{
    private VisibleObject mVisibleTarget;

    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }

    public void Enter(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
        if (mVisibleTarget != null)
        {
            agent.TargetTransform = mVisibleTarget.Object.transform;
            agent.StateMachine.OnChangeState(AIStateId.FollowTarget);
        }
    }

    public void Exit(AIAgent agent)
    {
        mVisibleTarget = null;
    }

    public void OnFOVEvent(AIAgent agent, int state, VisibleObject visibleObject)
    {
        mVisibleTarget = visibleObject;
    }
}