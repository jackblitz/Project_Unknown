﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimMotor : MonoBehaviour
{
    public float Speed = 0.05f;
    private Vector3 Direction;
    private float mLastAngle;

    public Vector3 Position
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // float Angle = CalculateRotationDestination();
        float currentAngle = Vector3.SignedAngle(transform.forward, Position, Vector3.up);
        float facingAngle = Vector3.SignedAngle(transform.forward, Direction, Vector3.up);   //Vector3.Angle(transform.forward, Direction);
        float clampedAngle = Mathf.Clamp(facingAngle, -140, 140);

        float Angle = clampedAngle;
      /*  //If we are flip minus counter clock wise to positive 
        if (clampedAngle < 0)
        {
            Angle = (360 - clampedAngle) * -1;
        }*/


        //TODO Smooth move from current angle to next
        Angle = Mathf.Lerp(currentAngle, Angle, Speed * Time.deltaTime);

        Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, Angle, transform.rotation.eulerAngles.z);
        Position = RotationUtils.RotatePointAroundPivot(transform.forward, Vector3.zero, rotationAngle.eulerAngles);
    }

    private float CalculateRotationDestination()
    {
        return Mathf.RoundToInt(Vector3.SignedAngle(transform.TransformDirection(Direction), transform.forward, Vector3.down));
    }

    public void setDirection(Vector3 direction)
    {
        Direction = direction;

       // Position = direction;
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