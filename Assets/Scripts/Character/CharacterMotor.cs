using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    public float Speed = 0.05f;
    private Vector3 Direction;

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
      Position = Vector3.Lerp(Position, Direction, Speed * Time.deltaTime);
    }

    public void setDirection(Vector3 direction)
    {
        Direction = direction;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Direction, .05f);
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Position, .05f);
    }
}
