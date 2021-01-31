using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMotion : MonoBehaviour
{
    public Quaternion InitalRotation;
    public Transform TargetBody;

    public ConfigurableJoint configurableJoint;
    // Start is called before the first frame update
    void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (TargetBody != null)
       {
            ConfigurableJointExtensions.SetTargetRotationLocal(configurableJoint, TargetBody.transform.localRotation, InitalRotation);
       }
     //   else
    //    {
     //       transform.localRotation = TargetBody.localRotation;
     //   }

       // configurableJoint.targetPosition = TargetBody.transform.position;
    }
}
