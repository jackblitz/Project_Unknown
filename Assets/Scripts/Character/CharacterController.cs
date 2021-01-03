using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class CharacterController : MonoBehaviour
{
    //Speed of Tween between idle/walk/run state. Smoothly move between speed 
    public float KeyFrameDelta = 3f;

    private CharacterAimMotor mAimMotor;
    private CharacterMotor mMovementMotor;
    private Animator mAnimator;

    private static int SPEED_HASH;
    private static int ANGLEVELOCITY_HASH;
    private float mLastSpeed;

    //Rig Controller
    public Rig mHiplayer;
    public Rig mAimlayer;
    // Duration for IK animation to new state
    public float AnimDuration = 0.3f;
    public float AimDuration = 0.8f;

    //Players aim state
    private AimState mAimState;


    public enum AimState
    {
        Idle = 1,
        Hip = 2,
        Aim = 3
    }

    public enum PlayerState
    {
        Exploration = 0,
        Cover = 1,
    }
    public PlayerState GetPlayerState
    {
        get; private set;
    }


    public void setAimState(AimState aimState)
    {
        mAimState = aimState;
    }

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAimMotor = GetComponent<CharacterAimMotor>();
        mMovementMotor = GetComponent<CharacterMotor>();

        SPEED_HASH = Animator.StringToHash("Speed");
        ANGLEVELOCITY_HASH = Animator.StringToHash("AngleVelocity");
    }

    private void Update()
    {
        switch (mAimState)
        {
            case AimState.Idle:
                mAimlayer.weight = Mathf.Lerp(mAimlayer.weight, 0, Time.deltaTime / AnimDuration);
                mHiplayer.weight = Mathf.Lerp(mHiplayer.weight, 0, Time.deltaTime / AnimDuration);
                break;
            case AimState.Hip:
                mAimlayer.weight = Mathf.Lerp(mAimlayer.weight, 0, Time.deltaTime / AnimDuration);
                mHiplayer.weight = Mathf.Lerp(mHiplayer.weight, 1, Time.deltaTime / AnimDuration);
                break;
            case AimState.Aim:
                mAimlayer.weight = Mathf.Lerp(mAimlayer.weight, 1, Time.deltaTime / AimDuration);
                mHiplayer.weight = Mathf.Lerp(mHiplayer.weight, 0, Time.deltaTime / AnimDuration);
                break;
        }

       OnUpdateChatacterPosition();
        
    }

    /**
    *  Send Input data updates to player controller.
    *  Animator controll is informed on changes to players speed
    */
    private void UpdateSpeed()
    {
        float speed = mMovementMotor.Position.magnitude;

        if (mMovementMotor.Position.magnitude > 0.1f)
        {
            speed += mMovementMotor.isRunning ? .8f : 0;
        }

        speed = Mathf.Lerp(mLastSpeed, speed, KeyFrameDelta * Time.deltaTime);

        //TODO This is not the best way to stop the animation controller in order to force is out of the walking state. We need to work our how much the player needs to move forward before snaping to cover
       if (GetPlayerState == PlayerState.Cover)
       {
            speed = 0;
       }

        mAnimator.SetFloat("Speed", speed);

        mLastSpeed = speed;
    }

    // Update is called once per frame
    void OnUpdateChatacterPosition()
    {
        UpdateSpeed();

        if (mMovementMotor.Position.magnitude > 0.1f)
        {
            transform.forward = mMovementMotor.Position;
        }

        if(GetPlayerState == PlayerState.Cover)
        {
            transform.forward = Vector3.back;
        }
    }

    public void onAttachedCover()
    {
        GetPlayerState = PlayerState.Cover;
        onUpdateAnimationLocomotion();
    }

    public void onDettachCover()
    {
        GetPlayerState = PlayerState.Exploration;
        onUpdateAnimationLocomotion();
    }

    public void onUpdateAnimationLocomotion()
    {
        mAnimator.SetInteger("LocomotionState", (int)GetPlayerState);
    }
}
