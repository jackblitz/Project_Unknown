using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine
{
    public AIState[] States;
    public AIAgent Agent;
    public AIStateId CurrentState;

    public AIStateMachine(AIAgent agent)
    {
        Agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateId)).Length;

        States = new AIState[numStates];
    }

    public void RegisterStates(AIState state)
    {
        int index = (int)state.GetId();
        States[index] = state;
    }

    public AIState GetState(AIStateId stateId)
    {
        int index = (int)stateId;
        return States[index];
    }

    public void Update()
    {
        GetState(CurrentState)?.Update(Agent);
    }

    public void OnChangeState(AIStateId newState)
    {
        GetState(CurrentState)?.Exit(Agent);
        CurrentState = newState;
        GetState(CurrentState)?.Enter(Agent);
    }
}