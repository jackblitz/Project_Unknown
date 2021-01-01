using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSpeed = 3;
    private Vector3 lookSmoothDirection;

    public CharacterAimMotor AimMotor;
    public CharacterMotor MovementMotor;

    private CharacterController mCharacterController;
    // Start is called before the first frame update

    PlayerInputActions mInput;

    [Header("Movement Settings")]
    private Vector3 mLastRawDirection = Vector3.forward;
    private Vector3 mCameraForward;
    private Vector3 mCameraRight;

    private Vector3 mCameraLookForward;
    private Vector3 mCameraLookRight;
    private Vector3 mLastLookDirection;


    /**
     * Player Look at Direction
     **/
    public Vector2 LookAtDirection
    {
        get;
        set;
    }

    /**
     * Player Look at Direction
     **/
    public Vector2 MoveToDirection
    {
        get;
        set;
    }

    /**
     * Is Player Running
     */
    public bool IsRunning
    {
        get;
        set;
    }
    public bool IsAiming { get; private set; }

    private void Awake()
    {
        mInput = new PlayerInputActions();

        mInput.PlayerControls.Move.performed += ctx => MoveToDirection = ctx.ReadValue<Vector2>();
        mInput.PlayerControls.LookAt.performed += ctx => LookAtDirection = ctx.ReadValue<Vector2>();

        mInput.PlayerControls.Run.performed += ctx => IsRunning = ctx.ReadValueAsButton();

        mInput.PlayerControls.ContextLock.performed += ctx => IsAiming = ctx.ReadValueAsButton();
    }
    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateMoveDirection();
        OnUpdateLookDirection();
        OnUpdateSpeed();

        OnUpdateAimState();
    }

    private void OnUpdateAimState()
    {
        if (MoveToDirection.magnitude < .1 && LookAtDirection.magnitude < .1)
        {
            mCharacterController.setAimState(CharacterController.AimState.Idle);
        }
        else
        {
            mCharacterController.setAimState(CharacterController.AimState.Hip);
        }

        if (IsAiming)
        {
            mCharacterController.setAimState(CharacterController.AimState.Aim);
        }
    }

    private void OnUpdateSpeed()
    {
        MovementMotor.isRunning = IsRunning;
    }

    private void OnUpdateLookDirection()
    {
        ////Looking
        Vector3 rawLookDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.y);

        //Only recalcuate forward direction if player moves direction
        if (Vector3.Distance(rawLookDirection, mLastLookDirection) > 0.2f)
        {
            mCameraLookForward = Camera.main.transform.forward;
            mCameraLookRight = Camera.main.transform.right;
            mLastLookDirection = rawLookDirection;
        }

        var moveLookDirection = (mCameraLookRight * rawLookDirection.x + mCameraLookForward * rawLookDirection.z).normalized;
        moveLookDirection.y = 0;

        if (moveLookDirection.magnitude > 0.1f)
            AimMotor.setDirection(moveLookDirection);
        else
            AimMotor.setDirection(transform.forward);

    }

    private void OnUpdateMoveDirection()
    {
        Vector3 rawDirection = new Vector3(MoveToDirection.x, 0, MoveToDirection.y);

       // float differenceX = Mathf.Abs(rawDirection.x - rawDirection.x);

        //Only recalcuate forward direction if player moves direction
        if (Vector3.Distance(rawDirection, mLastRawDirection) > 0.2f)
        {
            mCameraForward = Camera.main.transform.forward;
            mCameraRight = Camera.main.transform.right;

            mLastRawDirection = rawDirection;
        }

        var moveSmoothDirection = (mCameraRight * rawDirection.x + mCameraForward * rawDirection.z).normalized;
        moveSmoothDirection.y = 0;

        if (rawDirection.magnitude > 0.1f)
            MovementMotor.setDirection(moveSmoothDirection);
        else
            MovementMotor.setDirection(Vector3.zero);

      
    }

    private void OnEnable()
    {
        mInput.Enable();
    }

    private void OnDisable()
    {
        mInput.Disable();
    }
}
