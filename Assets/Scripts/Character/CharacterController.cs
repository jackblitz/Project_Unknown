using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class CharacterController : MonoBehaviour
{
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
    public float AnimDuration = 0.3f;
    public float AimDuration = 0.8f;
    private AimState mAimState;

    public enum AimState
    {
        Idle = 1,
        Hip = 2,
        Aim = 3
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
    }

    /**
    *  Send Input data updates to player controller.
    *  Animator controll is informed on changes to players speed
    */
    private void UpdateSpeed()
    {
        float speed = mMovementMotor.Position.magnitude;

        speed += mMovementMotor.isRunning ? .8f : 0;

        float newSpeed = Mathf.Lerp(mLastSpeed, speed, KeyFrameDelta * Time.deltaTime);
        mAnimator.SetFloat("Speed", newSpeed);

        mLastSpeed = newSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSpeed();

        if (mMovementMotor.Position.magnitude > 0.1f)
        {
            transform.forward = mMovementMotor.Position;
        }
    }
}
