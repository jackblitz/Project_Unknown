using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    private NavMeshAgent mAgent;
    private Animator mAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (mAgent.hasPath)
        {
            mAnimator.SetFloat("Speed", mAgent.velocity.magnitude);
        }
        else
        {
            mAnimator.SetFloat("Speed", 0);
        }
    }
}
