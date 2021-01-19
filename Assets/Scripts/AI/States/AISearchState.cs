using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static FieldOfView;

public class AISearchState : AIState
{
    public Vector3 LastKnownLocation = Vector3.zero;

    public void Enter(AIAgent agent)
    {
        agent.NavMeshAgent.SetDestination(LastKnownLocation);
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateId GetId()
    {
        return AIStateId.Search;
    }

    public void OnFOVEvent(AIAgent agent, int state, FieldOfView.VisibleObject visibleObject)
    {
        
    }

    public void Update(AIAgent agent)
    {
        if (agent.FOV.VisibleObjects.Count > 0)
        {
            VisibleObject visibleObject = agent.FOV.VisibleObjects[0];
            agent.TargetTransform = visibleObject.Object.transform;
            agent.StateMachine.OnChangeState(AIStateId.FollowTarget);
        }
        else if (agent.NavMeshAgent.remainingDistance <= 0.0f)
        {
            agent.StateMachine.OnChangeState(agent.InitState);
        }
    }
}

