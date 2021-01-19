using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static FieldOfView;

public class AIFollowTargetState : AIState
{
    float timer = 0.0f;

    public void Enter(AIAgent agent)
    {

    }

    public void Exit(AIAgent agent)
    {
       
       
    }

    public AIStateId GetId()
    {
        return AIStateId.FollowTarget;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (!agent.NavMeshAgent.hasPath)
        {
            agent.NavMeshAgent.destination = agent.TargetTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.TargetTransform.position - agent.NavMeshAgent.destination);

            direction.y = 0;
            if (direction.sqrMagnitude > agent.config.MaxDistance * agent.config.MaxDistance)
            {
                if (agent.NavMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.NavMeshAgent.destination = agent.TargetTransform.position;
                }
            }
            timer = agent.config.MaxTime;
        }
    }

    public void OnFOVEvent(AIAgent agent, int state, VisibleObject visibleObject)
    {
        switch (state)
        {
            case (int)FieldOfViewEvent.FieldOfViewEvents.LostTarget:
                AISearchState searchState = agent.StateMachine.GetState(AIStateId.Search) as AISearchState;
                searchState.LastKnownLocation = agent.TargetTransform.position;
                agent.StateMachine.OnChangeState(AIStateId.Search);
                break;
        }
    }
}
