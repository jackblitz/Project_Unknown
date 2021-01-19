using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static FieldOfView;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine StateMachine;
    public AIAgentConfig config;
    public WayPointHolder PatrolRoute;

    public AIStateId InitState;

    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Ragdoll Ragdoll;
    [HideInInspector] public UIHealthBar UI;
    [HideInInspector] public AIFOV FOV;

    [HideInInspector] public Transform TargetTransform;

    // Start is called before the first frame update
    void Start()
    {
        Ragdoll = GetComponent<Ragdoll>();
        UI = GetComponentInChildren<UIHealthBar>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<AIFOV>();

        StateMachine = new AIStateMachine(this);
        StateMachine.RegisterStates(new AIFollowTargetState());
        StateMachine.RegisterStates(new AIDeathState());
        StateMachine.RegisterStates(new AIIdleState());
        StateMachine.RegisterStates(new AIPatrolState());
        StateMachine.RegisterStates(new AISearchState());
        StateMachine.OnChangeState(InitState);

        FOV.Event.AddListener(OnFOVEvent);

        setFOVConfig();
    }

    private void setFOVConfig()
    {
        FOV.TargetMask = config.TargetMask;
        FOV.ViewAngle = config.ViewAngle;
        FOV.ViewRadius = config.ViewRadius;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.Update();
    }

    private void OnFOVEvent(int state, VisibleObject visibleObject)
    {
        StateMachine.OnFOVEvent(state, visibleObject);
    }
}
