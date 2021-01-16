using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterLocomotion : MonoBehaviour
{
    //Speed of Tween between idle/walk/run state. Smoothly move between speed 
    public float KeyFrameDelta = 3f;
    public float Gravity = 6f;
    public float StepOffset = 0.3f;
    public float JumpHieght = 2.5f;
    public float AirControl = 2.5f;
    public float JumpDamp = 0.4f;
    public float GroundSpeed = 3f;

    private CharacterAimMotor mAimMotor;
    private CharacterMotor mMovementMotor;
    private WeaponController mWeaponController;
    private CharacterController mCharacterController;

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

    private Vector3 rootMotion;
    private Vector3 velocity;

    public enum AimState
    {
        Hip = 1,
        Aim = 2
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
        mWeaponController = GetComponent<WeaponController>();
        mCharacterController = GetComponent<CharacterController>();
        SPEED_HASH = Animator.StringToHash("Speed");
        ANGLEVELOCITY_HASH = Animator.StringToHash("AngleVelocity");
    }

    private void Update()
    {
        UpdateSpeed();
    }

    private void OnAnimatorMove()
    {
        rootMotion += mAnimator.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            OnUpdateOnGround();
        }


        OnUpdateChatacterPosition();
    }

    private void OnUpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * GroundSpeed;
        Vector3 stepDownAmount = Vector3.down * StepOffset;
        mCharacterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!mCharacterController.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= Gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        mCharacterController.Move(velocity);
        isJumping = mCharacterController.isGrounded;
        rootMotion = Vector3.zero;
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * mMovementMotor.Position.y) + (transform.right * mMovementMotor.Position.x)) * (AirControl / 100);
    }

    public void OnJump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * Gravity * JumpHieght);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = mAnimator.velocity * JumpDamp * GroundSpeed;
        velocity.y = jumpVelocity;
    }

    private void LateUpdate()
    {
        switch (mAimState)
        {
            case AimState.Hip:
                mAimlayer.weight = Mathf.Lerp(mAimlayer.weight, 0, Time.deltaTime / AnimDuration);
               // mHiplayer.weight = Mathf.Lerp(mHiplayer.weight, 1, Time.deltaTime / AnimDuration);
                break;
            case AimState.Aim:
                mAimlayer.weight = Mathf.Lerp(mAimlayer.weight, 1, Time.deltaTime / AimDuration);
              //  mHiplayer.weight = Mathf.Lerp(mHiplayer.weight, 0, Time.deltaTime / AnimDuration);
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
        //GroundSpeed = speed + 1;

         mLastSpeed = speed;
    }

    // Update is called once per frame
    void OnUpdateChatacterPosition()
    {
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

    public void OnPullTrigger()
    {
        if(mWeaponController != null)
            mWeaponController.OnPullTrigger();
    }

    public void OnReleaseTrigger()
    {
        if (mWeaponController)
        {
            mWeaponController.OnReleaseTrigger();
        }
    }

    public void OnReload()
    {
        if (mWeaponController)
        {
            mWeaponController.OnReload();
        }
    }

    private ItemPickUp mInteractableItem;
    private bool isJumping;

    public void OnTriggerItemEntered(GameObject gameObject)
    {
        mInteractableItem = gameObject.GetComponent<ItemPickUp>();
    }

    public void OnTrgggerItemExited(GameObject gameObject)
    {
        mInteractableItem = null;
    }

    public bool OnInteractWithItem()
    {
        if(mInteractableItem != null)
        {
            Item item = mInteractableItem.OnItemInteract();
     
            mWeaponController.OnEquipWeapon((WeaponItem)item);
            return true;
        }
        return false;
    }

    public void OnNextWeapon()
    {
        if (mWeaponController != null)
            mWeaponController.OnNextWeapon();
    }

    public void OnPreviousWeapon()
    {
        if (mWeaponController != null)
            mWeaponController.OnPreviousWeapon();
    }

    public void OnHolsterWeapon()
    {
        if (mWeaponController != null)
            mWeaponController.HolsterActiveWeapon();
    }

}
