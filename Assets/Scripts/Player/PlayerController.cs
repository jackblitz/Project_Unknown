using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public CharacterAimMotor AimMotor;
    public CharacterMotor MovementMotor;

    private CharacterController mCharacterController;
    // Start is called before the first frame update

    PlayerInputActions mInput;

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

    private void Awake()
    {
        mInput = new PlayerInputActions();

        mInput.PlayerControls.Move.performed += ctx => MoveToDirection = ctx.ReadValue<Vector2>();
        mInput.PlayerControls.LookAt.performed += ctx => LookAtDirection = ctx.ReadValue<Vector2>();

        mInput.PlayerControls.Run.performed += ctx => IsRunning = ctx.ReadValueAsButton();
    }
    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rawDirection = new Vector3(MoveToDirection.x, 0, MoveToDirection.y);
        MovementMotor.setDirection(rawDirection);

        Vector3 rawLookDirection = new Vector3(LookAtDirection.x, 0, LookAtDirection.y);
        AimMotor.setDirection(rawLookDirection);
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
