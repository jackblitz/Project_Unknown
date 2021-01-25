using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMotor))]
public class CharacterPhysicsController : MonoBehaviour
{
    public Rigidbody mBody;
    private CharacterMotor mMoveMotor;

    public Animator mAnimator;
    public CharacterController mCharacterController;

    // Start is called before the first frame update
    void Start()
    {
        //mBody = GetComponent<Rigidbody>();
        mMoveMotor = GetComponent<CharacterMotor>();
      // mCharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (mBody.velocity.magnitude > 3f)
        {
           // mBody.velocity = Vector3.ClampMagnitude(mBody.velocity, 3.2f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mBody.AddForce(mMoveMotor.Direction * 2.2f, ForceMode.Impulse);
       // mCharacterController.Move(mMoveMotor.Direction);
        //transform.forward = mBody.velocity.normalized;
        mAnimator.SetFloat("Speed", mMoveMotor.Direction.magnitude);
        Debug.Log(mMoveMotor.Position);
    }
}
