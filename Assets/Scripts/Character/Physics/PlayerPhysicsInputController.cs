using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class PlayerPhysicsInputController : MonoBehaviour
{
    private PlayerInputActions mInput;

    private CharacterMotor mMotor;
    private CharacterAimMotor mAimMotor;
    private CharacterPhysicsController mCharacterController;

    float Direction;
    float Speed = 0f;
    public float DirectionSpeed = 3f;
    /**
    * Player Look at Direction
    **/
    private Vector2 InputLookAtDirection
    {
        get;
        set;
    }

    /**
     * Players Input Direction
     **/
    private Vector2 InputMoveToDirection
    {
        get;
        set;
    }

    private bool IsFiring = false;

    // Start is called before the first frame update
    void Awake()
    {
        mInput = new PlayerInputActions();

        mInput.PlayerControls.Move.performed += ctx => InputMoveToDirection = ctx.ReadValue<Vector2>();
        mInput.PlayerControls.LookAt.performed += ctx => InputLookAtDirection = ctx.ReadValue<Vector2>();

        mInput.PlayerControls.ContextAttack.performed += ctx => OnAttack(ctx.ReadValueAsButton());

        mInput.PlayerControls.Interact.performed += ctx => OnInteract(ctx.ReadValueAsButton());

        mInput.PlayerControls.WeaponWheelRight.performed += ctx => OnRightWeaponWheel(ctx.ReadValueAsButton());
        mInput.PlayerControls.WeaponWheelLeft.performed += ctx => OnLeftWeaponWheel(ctx.ReadValueAsButton());
        mInput.PlayerControls.HolsterWeapon.performed += ctx => OnHolsterWeapon(ctx.ReadValueAsButton());

        mMotor = GetComponent<CharacterMotor>();
        mAimMotor = GetComponent<CharacterAimMotor>();
        mCharacterController = GetComponent<CharacterPhysicsController>();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateLookDirection();
        OnUpdateMoveDirection();
    }

    void LateUpdate()
    {
        if (IsFiring)
        {
            mCharacterController.OnPullTrigger();
        }
        else
        {
            mCharacterController.OnReleaseTrigger();
        }
    }

    public void OnAttack(bool isFiring)
    {
        IsFiring = isFiring;
    }

    private void OnInteract(bool value)
    {
        if (value)
        {
            if (!mCharacterController.OnInteractWithItem())
            {
                mCharacterController.OnReload();
            }
        }
    }

    private void OnRightWeaponWheel(bool value)
    {
        if (value)
        {
            mCharacterController.OnNextWeapon();
        }
    }

    private void OnLeftWeaponWheel(bool value)
    {
        if (value)
        {
            mCharacterController.OnPreviousWeapon();
        }
    }
    private void OnHolsterWeapon(bool value)
    {
        if (value)
        {
            mCharacterController.OnHolsterWeapon();
        }
    }

    public void OnUpdateMoveDirection()
    {
        Vector3 rawDirection = new Vector3(InputMoveToDirection.x, 0, InputMoveToDirection.y);

        var moveSmoothDirection = (Camera.main.transform.right * rawDirection.x + Camera.main.transform.forward * rawDirection.z).normalized;
        moveSmoothDirection.y = 0;

       if (rawDirection.magnitude > 0.1f)
          mMotor.setInputDirection(moveSmoothDirection);
        else
           mMotor.setInputDirection(Vector3.zero);
    }

    private void OnUpdateLookDirection()
    {
        ////Looking
        Vector3 rawLookDirection = new Vector3(InputLookAtDirection.x, 0, InputLookAtDirection.y);


        var moveLookDirection = (Camera.main.transform.right * rawLookDirection.x + Camera.main.transform.forward * rawLookDirection.z).normalized;
        moveLookDirection.y = 0;


        // AimMotor.setDirection(mAutoAim.getActiveTargetDirection());
        if (rawLookDirection.magnitude > 0.1f)
            mAimMotor.setDirection(moveLookDirection);
        else
            mAimMotor.setDirection(transform.forward);

    }


    private void OnEnable()
    {
        if(mInput != null)
            mInput.Enable();
    }

    private void OnDisable()
    {
        if (mInput != null)
            mInput.Disable();
    }
}
