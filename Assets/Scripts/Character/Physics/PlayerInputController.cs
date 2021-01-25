using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputActions mInput;
    private Rigidbody mBody;

    private CharacterMotor mMotor;
    /**
    * Player Look at Direction
    **/
    public Vector2 LookAtDirection
    {
        get;
        set;
    }

    /**
     * Players Input Direction
     **/
    public Vector2 MoveToDirection
    {
        get;
        set;
    }
    // Start is called before the first frame update
    void Awake()
    {
        mInput = new PlayerInputActions();

        mInput.PlayerControls.Move.performed += ctx => MoveToDirection = ctx.ReadValue<Vector2>();
        mInput.PlayerControls.LookAt.performed += ctx => LookAtDirection = ctx.ReadValue<Vector2>();

        mBody = GetComponent<Rigidbody>();
        mMotor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateMoveDirection();
    }

    public void OnUpdateMoveDirection()
    {
        Vector3 rawDirection = new Vector3(MoveToDirection.x, 0, MoveToDirection.y);

        var moveSmoothDirection = (Camera.main.transform.right * rawDirection.x + Camera.main.transform.forward * rawDirection.z).normalized;
        moveSmoothDirection.y = 0;

       // if (rawDirection.magnitude > 0.1f)
           mMotor.setDirection(moveSmoothDirection);
       // else
       //     mMotor.setDirection(Vector3.zero);
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
