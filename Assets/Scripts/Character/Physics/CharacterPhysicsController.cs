using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMotor))]
public class CharacterPhysicsController : MonoBehaviour
{
    private Rigidbody mBody;
    private CharacterMotor mMoveMotor;
    // Start is called before the first frame update
    void Start()
    {
        mBody = GetComponent<Rigidbody>();
        mMoveMotor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        if (mBody.velocity.magnitude > 4f)
        {
            mBody.velocity = Vector3.ClampMagnitude(mBody.velocity, 4f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mBody.AddForce(mMoveMotor.Direction, ForceMode.Impulse);

        //transform.forward = mBody.velocity.normalized;

        Debug.Log(mMoveMotor.Position);
    }
}
