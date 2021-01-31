using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class CharacterPhysicsController : MonoBehaviour
{
    public Rigidbody mBody;
    private CharacterMotor mMoveMotor;

    public Animator mAnimator;
    public CharacterController mCharacterController;

    // Start is called before the first frame update
    void Start()
    {
       mBody = GetComponent<Rigidbody>();
       mMoveMotor = GetComponent<CharacterMotor>();
       mCharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
       // if (mBody.velocity.magnitude > 3f)
       // {
           // mBody.velocity = Vector3.ClampMagnitude(mBody.velocity, 3.2f);
       // }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mCharacterController.Move(mMoveMotor.Direction / 28.2f);
       // mBody.position = mCharacterController.transform.position;
   // mBody.AddForce(mMoveMotor.Direction * 2.2f, ForceMode.Impulse);
   // mCharacterController.Move(mMoveMotor.Direction);
   //transform.forward = mBody.velocity.normalized;
        mAnimator.SetFloat("Speed", mMoveMotor.Direction.magnitude);
        Debug.Log(mMoveMotor.Direction.magnitude);
    }

  /*  private void OnUpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * GroundSpeed;
        Vector3 stepDownAmount = Vector3.down * StepOffset;
        mCharacterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!mCharacterController.isGrounded)
        {
            SetInAir(0);
        }
    }*/
}
