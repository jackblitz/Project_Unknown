using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{
    public CharacterAimMotor mAimMotor;
    private Vector3 mOffset;

    // Start is called before the first frame update
    void Start()
    {
        mOffset = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = (mAimMotor.transform.position + mAimMotor.Position * 5) + mOffset;
        transform.forward = mAimMotor.Position;
    }
}
