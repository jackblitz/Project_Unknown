using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimMotor : MonoBehaviour
{
    public float Speed = 0.05f;
    private Vector3 Direction;
    private float mLastAngle;

    public GameObject FocusObject;

    public bool ignoreTarget = false;

    public Vector3 Position
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void setFocusPoint(GameObject gameObject)
    {
        FocusObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Speed;

        if (FocusObject != null)
        {
            Vector3 focusedPosition = (FocusObject.transform.position - transform.position).normalized;
            Direction = new Vector3(focusedPosition.x, 0, focusedPosition.z);

        }

        float currentAngle = Vector3.SignedAngle(transform.forward, Position, Vector3.up);
        float facingAngle = Vector3.SignedAngle(transform.forward, Direction, Vector3.up);   //Vector3.Angle(transform.forward, Direction);

        float clampedAngle = Mathf.Clamp(facingAngle, -145, 145);

        float Angle = clampedAngle;
 
        Angle = Mathf.Lerp(currentAngle, Angle, speed * Time.deltaTime);

        //Remove smooth move and snapp to target
        if (FocusObject != null)
        {
            Angle = clampedAngle;
        }
        

        Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, Angle, transform.rotation.eulerAngles.z);
        Position = RotationUtils.RotatePointAroundPivot(transform.forward, Vector3.zero, rotationAngle.eulerAngles);

    }

    private float CalculateRotationDestination()
    {
        return Mathf.RoundToInt(Vector3.SignedAngle(transform.TransformDirection(Direction), transform.forward, Vector3.down));
    }

    public void setDirection(Vector3 direction)
    {
        /*if(Mathf.Abs(Vector3.Distance(direction, Direction)) > 1.1f)
        {
            FocusObject = null;
        }*/
        //Loose focus to object if player moves direction
        Direction = direction;       
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Direction, .05f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Position, .05f);
    }
}
