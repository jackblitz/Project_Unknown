using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] mRigidbodies;
    Animator mAnimator;
    private Rigidbody mLastHitBodyPart;

    public int SolverInterations = 8;
    public int SolverVelocityInterations = 8;
    public float MaxAngularVelocity = 20f;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbodies = GetComponentsInChildren<Rigidbody>();
        mAnimator = GetComponent<Animator>();

        //OnDeactivateRagdoll();

        foreach(Rigidbody rb in mRigidbodies)
        {
            rb.solverIterations = SolverInterations;
            rb.solverVelocityIterations = SolverVelocityInterations;
            rb.maxAngularVelocity = MaxAngularVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeactivateRagdoll()
    {
        foreach(Rigidbody body in mRigidbodies)
        {
            body.isKinematic = true;
        }

        mAnimator.enabled = true;
    }

    public void OnActivateRagdoll()
    {
        foreach (Rigidbody body in mRigidbodies)
        {
            body.isKinematic = false;
        }

        mAnimator.enabled = false;
    }

    public void SetOnLastHitBody(Rigidbody rigidbody)
    {
        mLastHitBodyPart = rigidbody;
    }

    public void ApplyForce(Vector3 force)
    {
        mLastHitBodyPart.AddForce(force, ForceMode.VelocityChange);
    }
}
