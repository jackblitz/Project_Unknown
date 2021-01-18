using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FieldOfView;

public class AIIdleState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }

    public void Enter(AIAgent agent)
    {
    }

    public void Update(AIAgent agent)
    {
        VisibleObject obj = agent.FOV.GetVisibleObject();

        if (obj != null)
        {
            agent.TargetTransform = obj.Object.transform;
            agent.StateMachine.OnChangeState(AIStateId.ChasePlayer);
        }
    }

    public void Exit(AIAgent agent)
    {
    }
}