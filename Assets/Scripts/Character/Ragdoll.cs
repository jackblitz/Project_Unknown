using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] mRigidbodies;
    Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbodies = GetComponentsInChildren<Rigidbody>();
        mAnimator = GetComponent<Animator>();

        OnDeactivateRagdoll();
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

    public void ApplyForce(Vector3 force)
    {
        //var rigidBody = mAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        //rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
}
