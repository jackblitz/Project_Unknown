using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    private NavMeshAgent mAgent;
    private Animator mAnimator;

    public Transform Target;

    public float MaxTime = 1f;
    public float MaxDistance = 1.0f;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0.0f)
        {
            float distance = (Target.transform.position - mAgent.destination).sqrMagnitude;
            if (distance > MaxDistance * MaxDistance)
            {
                mAgent.destination = Target.position;
            }

            timer = MaxTime;
        }

        mAnimator.SetFloat("Speed", mAgent.velocity.magnitude);
    }
}
