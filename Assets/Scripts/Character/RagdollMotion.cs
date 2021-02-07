using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMotion : MonoBehaviour
{
    public Quaternion InitalRotation;
    public Transform TargetBody;
    private Rigidbody rigidbody;
    public Vector3 Anchor;
    public ConfigurableJoint configurableJoint;
    private Vector3 StartPosition;


    public bool UpdatePositon = false;
    // Start is called before the first frame update
    void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
        rigidbody = GetComponent<Rigidbody>();
        StartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

 
       if (TargetBody != null && configurableJoint != null)
       {
            ConfigurableJointExtensions.SetTargetRotationLocal(configurableJoint, TargetBody.transform.localRotation, InitalRotation);
       }
        //   else
        //    {
        //       transform.localRotation = TargetBody.localRotation;
        //   }

        // transform.r - (transform.position - TargetBody.transform.position);
        if (UpdatePositon)
        {
            // configurableJoint.targetPosition = mStartPosition + (mStartPosition - TargetBody.transform.position);
            // transform.localPosition = TargetBody.transform.localPosition;
            //rigidbody.MoveRotation(TargetBody.localRotation);
            //rigidbody.MoveRotation(configurableJoint.targetRotation);

            //   Vector3 difference = StartPosition - new Vector3(TargetBody.localPosition.y, TargetBody.localPosition.x, TargetBody.localPosition.z);

               Vector3 reflect = Vector3.Reflect(TargetBody.localPosition, Vector3.right);
               reflect = Vector3.Reflect(reflect, Vector3.forward);
              reflect = Vector3.Reflect(reflect, Vector3.up);

            // Vector3 difference = StartPosition - reflect;// new Vector3(TargetBody.localPosition.y, TargetBody.localPosition.x, TargetBody.localPosition.z) ;

            //configurableJoint.connectedAnchor = Anchor;
            configurableJoint.targetPosition = reflect;

          //  Debug.Log(difference) ;
        }
    }
}
