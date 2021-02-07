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


    // Start is called before the first frame update
    void Awake()
    {
        mInput = new PlayerInputActions();

        mInput.PlayerControls.Move.performed += ctx => InputMoveToDirection = ctx.ReadValue<Vector2>();
        mInput.PlayerControls.LookAt.performed += ctx => InputLookAtDirection = ctx.ReadValue<Vector2>();

        mMotor = GetComponent<CharacterMotor>();
        mAimMotor = GetComponent<CharacterAimMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdateMoveDirection();
    }

    public void OnUpdateMoveDirection()
    {
        Vector3 rawDirection = new Vector3(InputMoveToDirection.x, 0, InputMoveToDirection.y);

        var moveSmoothDirection = (Camera.main.transform.right * rawDirection.x + Camera.main.transform.forward * rawDirection.z).normalized;
        moveSmoothDirection.y = 0;

       if (rawDirection.magnitude > 0.1f)
          mMotor.setDirection(moveSmoothDirection);
        else
           mMotor.setDirection(Vector3.zero);
    }

    private void MovementtoWorldSpace(Transform root, Transform camera, ref Vector3 direction, ref float directionAngle, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;

        speedOut = direction.sqrMagnitude;

        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0;

        Quaternion referntialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        direction = referntialShift * direction;
        Vector3 axisSign = Vector3.Cross(direction, rootDirection);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), direction, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
        //Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), direction, Color.blue);

        float angleRootToMOve = Vector3.Angle(rootDirection, direction) * (axisSign.y >= 0 ? -1f : 1f);

        angleRootToMOve /= 180f;

        directionAngle = angleRootToMOve * DirectionSpeed;

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
