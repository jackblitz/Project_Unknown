using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class CharacterController : MonoBehaviour
{
    private CharacterAimMotor mAimMotor;
    private CharacterMotor mMovementMotor;
    private Animator mAnimator;

    private static int SPEED_HASH;
    private static int ANGLEVELOCITY_HASH;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAimMotor = GetComponent<CharacterAimMotor>();
        mMovementMotor = GetComponent<CharacterMotor>();

        SPEED_HASH = Animator.StringToHash("Speed");
        ANGLEVELOCITY_HASH = Animator.StringToHash("AngleVelocity");
    }

    /**
    *  Send Input data updates to player controller.
    *  Animator controll is informed on changes to players speed
    */
    private void UpdateSpeed()
    {
        mAnimator.SetFloat("Speed", mMovementMotor.Position.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        //transform.LookAt(mMovementMotor.Position);
    }
}
