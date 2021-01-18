using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
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
        return AIStateId.ChasePlayer;
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
}
