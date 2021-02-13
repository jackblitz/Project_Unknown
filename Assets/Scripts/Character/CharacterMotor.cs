using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    public float Dampening = 0.05f;
    private Vector3 InputDirection;

    public Camera RelativeCamera;

    public bool isRunning {
        get;
        set;
    }

    public Vector3 Position
    {
        get;
        set;
    }

    public Vector3 Direction
    {
        get;
        set;
    }

    public float Speed { get; set; }

    /// <summary>
    /// Is the direction relative to it current position
    /// </summary>

    public bool isRelative = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dampVel = Vector3.Lerp(Position, InputDirection, Dampening * Time.deltaTime);

        Speed = dampVel.sqrMagnitude;

        if (isRelative)
        {
            float directHor = 0f;
            float angle = 0f;

            Position = StickToWorldspace(dampVel, ref directHor, ref angle);

            Direction = new Vector3(directHor, 0, dampVel.z);
        }
        else
        {
            Position = dampVel;
            Direction = dampVel;
        }
    }

    public void setInputDirection(Vector3 direction)
    {
        InputDirection = direction;
    }

    public Vector3 StickToWorldspace(Vector3 position, ref float directionOut, ref float angleOut)
    {
        Vector3 rootDirection = transform.forward;

        // Get camera rotation
        Vector3 CameraDirection = RelativeCamera.transform.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * position;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), rootDirection, Color.magenta);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), position, Color.blue);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), axisSign, Color.red);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        angleOut = angleRootToMove;
  
        angleRootToMove /= 180f;

        directionOut = angleRootToMove * position.sqrMagnitude;

        return moveDirection; 
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + InputDirection, .05f);
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Position, .05f);
    }
}
