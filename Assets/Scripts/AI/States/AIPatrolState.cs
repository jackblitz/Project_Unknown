using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FieldOfView;

public class AIPatrolState : AIState
{
    private VisibleObject mVisibleTarget;
    private WayPointHolder mWayPointHolder;

    private int CurrentIndex = -1;

    public AIStateId GetId()
    {
        return AIStateId.Patrol;
    }

    public void Enter(AIAgent agent)
    {
        mWayPointHolder = agent.PatrolRoute;

        agent.NavMeshAgent.SetDestination(getNextWayPoint().transform.position);
    }

    public void Update(AIAgent agent)
    {
        if (mVisibleTarget != null)
        {
            agent.TargetTransform = mVisibleTarget.Object.transform;
            agent.StateMachine.OnChangeState(AIStateId.FollowTarget);
        }

        if(agent.NavMeshAgent.remainingDistance <= 0.0f)
        {
            WayPoint way = getNextWayPoint();
            agent.NavMeshAgent.speed = way.Speed;
            agent.NavMeshAgent.SetDestination(way.transform.position);
        }
    }

    public WayPoint getNextWayPoint()
    {
        CurrentIndex++;

        if(mWayPointHolder.WayPoints.Length <= CurrentIndex)
        {
            CurrentIndex = 0;
        }

        return mWayPointHolder.WayPoints[CurrentIndex];
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

