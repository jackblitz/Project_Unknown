using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterAimMotor))]
public class CharacterPhysicsController : MonoBehaviour
{
    /// <summary>
    /// Character collliders and physics
    /// </summary>
    public CharacterController mCharacterController;
    public Transform PoseHips;
    public Rigidbody RagdollHips;
    public Rigidbody mBody;

    //Chartacters AIM and Movement Direction
    private CharacterMotor mMoveMotor;
    private CharacterAimMotor mAimMotor;

    //Pose controller for playing the animation;
    private AnimatorPoseController mAnimatorPoseController;

    // Controller for updated the active ragdoll state
    private ActiveRagdollController mActiveRagdolController;


    // Start is called before the first frame update
    void Start()
    {
       mBody = GetComponent<Rigidbody>();
       mMoveMotor = GetComponent<CharacterMotor>();
       mAimMotor = GetComponent<CharacterAimMotor>();
       mCharacterController = GetComponent<CharacterController>();
       mAnimatorPoseController = GetComponentInChildren<AnimatorPoseController>();
       mActiveRagdolController = GetComponentInChildren<ActiveRagdollController>();

    }

    private void Update()
    {
        mAnimatorPoseController.SetSpeed(mMoveMotor.Direction.sqrMagnitude);
        Debug.Log(mMoveMotor.Direction.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        mCharacterController.Move(mAnimatorPoseController.RootMotion);
        mAnimatorPoseController.RootMotion = Vector3.zero;
    }

}

/* OLD CODE
//   private void Update()
//    {
// if (mBody.velocity.magnitude > 3f)
// {
// mBody.velocity = Vector3.ClampMagnitude(mBody.velocity, 3.2f);
// }
//   }

   // Update is called once per frame
   ///  void FixedUpdate()
  / / {
        //   mCharacterController.Move(mMoveMotor.Direction / 28.2f);
        // mBody.position = mCharacterController.transform.position;
        // mBody.AddForce(mMoveMotor.Direction * 2.2f, ForceMode.Impulse);
        // mCharacterController.Move(mMoveMotor.Direction);
        //transform.forward = mBody.velocity.normalized;

/* mAnimatorPoseController.setSpeed(mMoveMotor.Direction.sqrMagnitude);
 mBody.MovePosition(mBody.position + (mAnimatorPoseController.RootMotion * GroundSpeed));

 float gotoAngle = Vector3.SignedAngle(transform.forward, mMoveMotor.Position, Vector3.up);

 if (mMoveMotor.Position.magnitude > 0.1f)
 {
     Quaternion rotationAngle = Quaternion.Euler(transform.rotation.eulerAngles.x, gotoAngle, transform.rotation.eulerAngles.z);
     // transform.rotation = rotationAngle;
     //mBody.MoveRotation(rotationAngle);
 }

 mAnimatorPoseController.RootMotion = Vector3.zero;
 Debug.Log(mMoveMotor.Direction.magnitude);*/

// RagdollHips.MovePosition(RagdollHips.transform.position + PoseHips.transform.position);  
//   RagdollHips.
//RagdollHips.transform.position = PoseHips.transform.localPosition;
// }

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