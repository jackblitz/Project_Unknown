using System;
using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{
    private Animator animator;
    private FixedJoint fixedJoint;

    public Rigidbody CharacterRigidbody;

    public Rigidbody mHipRigidBody;

    public Animator StaticAnimator;

    [Header("--- UPRIGHT TORQUE ---")]
    public float uprightTorque = 5000;
    [Tooltip("Defines how much torque percent is applied given the inclination angle percent [0, 1]")]
    public AnimationCurve uprightTorqueFunction;
    public float rotationTorque = 500;

    [Header("Hip settings")]
    [SerializeField] public RagdolLimbState HipState = new RagdolLimbState();
    [SerializeField] public RagdollLimb Hip;

    [Header("Right Arm Settings")]
    [SerializeField] public RagdolLimbState RightArmLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] RightArmLimbs = new RagdollLimb[3];

    [Header("Left Arm Settings")]
    [SerializeField] public RagdolLimbState LeftArmLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] LeftArmLimbs;

    [Header("Head Settings")]
    [SerializeField] public RagdolLimbState HeadLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] HeadLimbs;

    [Header("Spine Settings")]
    [SerializeField] public RagdolLimbState SpineLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] SpineLimbs;

    [Header("Right Leg Settings")]
    [SerializeField] public RagdolLimbState RightLegLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] RightLefLimbs;

    [Header("Left Leg Settings")]
    [SerializeField] public RagdolLimbState LeftLegLimbState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] LeftLegLimbs;

    public HumanBodyBones RemovehumanBodyBonesId;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fixedJoint = GetComponent<FixedJoint>();


        foreach (RagdollLimb ragdollLimb in RightArmLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in LeftArmLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

        InitRagdoll(Hip);

        foreach (RagdollLimb ragdollLimb in HeadLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in SpineLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in RightLefLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in LeftLegLimbs)
        {
            InitRagdoll(ragdollLimb);
        }

       // OnRagDollStateChanged();

    }

    private void FixedUpdate()
    {
        //OnRagDollStateChanged();

        OnRemoveLimb(RemovehumanBodyBonesId);

        var balancePercent = Vector3.Angle(Hip.transform.up,
                                     Vector3.up) / 180;
        balancePercent = uprightTorqueFunction.Evaluate(balancePercent);
        var rot = Quaternion.FromToRotation(mHipRigidBody.transform.up,
                                             Vector3.forward).normalized;

       // mHipRigidBody.AddTorque(new Vector3(rot.x, rot.y, rot.z)
       //                                             * uprightTorque * balancePercent);

        var directionAnglePercent = Vector3.SignedAngle(Hip.transform.forward,
                            Vector3.forward, Vector3.up) / 180;
      // mHipRigidBody.AddRelativeTorque(0, directionAnglePercent * rotationTorque, 0);
    }

    private void OnRagDollStateChanged()
    { 
       foreach (RagdollLimb ragdollLimb in RightArmLimbs)
        {
           RightArmLimbState.UpdateState(ragdollLimb);
           RightArmLimbState.UpdateWeight(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in LeftArmLimbs)
        {
            LeftArmLimbState.UpdateState(ragdollLimb);
            LeftArmLimbState.UpdateWeight(ragdollLimb);
        }


       HipState.UpdateWeight(Hip);
        

        foreach (RagdollLimb ragdollLimb in HeadLimbs)
        {
            HeadLimbState.UpdateState(ragdollLimb);
            HeadLimbState.UpdateWeight(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in SpineLimbs)
        {
            SpineLimbState.UpdateState(ragdollLimb);
            SpineLimbState.UpdateWeight(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in RightLefLimbs)
        {
            RightLegLimbState.UpdateState(ragdollLimb);
            //RightLegLimbState.UpdateWeight(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in LeftLegLimbs)
        {
            LeftLegLimbState.UpdateState(ragdollLimb);
           // LeftLegLimbState.UpdateWeight(ragdollLimb);
        }
    }

    public void OnEnableFullRagdoll()
    {
        fixedJoint.connectedBody = null;
    }

    public void OnActiveRagdoll()
    {
        fixedJoint.connectedBody = CharacterRigidbody;
    }

    public void OnRemoveLimb(HumanBodyBones humanBodyBonesId)
    {
        Transform boneTransform = animator.GetBoneTransform(humanBodyBonesId);

        if (boneTransform == null)
            return;

        ConfigurableJoint configurableJoint = boneTransform.GetComponent<ConfigurableJoint>();
        CharacterJoint characterJoint = boneTransform.GetComponent<CharacterJoint>();

        OnRemoveMotionControler(boneTransform);

        //Remove all the motion controllers from limbs
        for (int i = 0; i < boneTransform.childCount; i++){
            Transform children = boneTransform.GetChild(i);
            OnRemoveMotionControler(children);

            //Allow childrend limbs to have weight into the ragdoll
            CharacterJoint joint = children.GetComponent<CharacterJoint>();
            if (joint != null)
            {
                joint.connectedMassScale = 1;
            }

            ConfigurableJoint configurableJoint1 = children.GetComponent<ConfigurableJoint>();
            if (configurableJoint1 != null)
            {
                OnRemoveWeight(configurableJoint1);
               // Destroy(configurableJoint1);
            }
        }

        if (configurableJoint != null)
        {
           Destroy(configurableJoint);
        }

        if (characterJoint != null)
        {
            Destroy(characterJoint);
        }

        boneTransform.parent = null;
    }

    private void OnRemoveMotionControler(Transform transform)
    {
        RagdollMotion ragdollMotion = transform.GetComponent<RagdollMotion>();

        if (ragdollMotion != null)
        {
            Destroy(ragdollMotion);
        }
    }

    public void OnRemoveWeight(ConfigurableJoint joint)
    {
        if (joint == null)
            return;

        var xDrive = joint.angularXDrive;

        xDrive.positionSpring = 0;
        xDrive.positionDamper = 0;

        var YXDrive = joint.angularYZDrive;

        YXDrive.positionSpring = 0;
        YXDrive.positionDamper = 0;

        joint.angularXDrive = xDrive;
        joint.angularYZDrive = YXDrive;

    }


    /// <summary>
    /// Creates an instance of ragdoll limb based on humand bone id
    /// </summary>
    /// <param name="anim">Animator Compononet which contains bone data</param>
    /// <param name="humanBoneId">ID of the bone you want</param>
    /// <returns></returns>
    public void InitRagdoll(RagdollLimb limb)
    {
        Transform boneTransform = animator.GetBoneTransform(limb.HumanBoneId);

        Transform animationboneTransform = StaticAnimator.GetBoneTransform(limb.HumanBoneId);

        if (boneTransform != null)
        {
            limb.transform = boneTransform;
            limb.rigidbody = boneTransform.GetComponent<Rigidbody>();
            limb.configurableJoint = boneTransform.GetComponent<ConfigurableJoint>();
            limb.characterJoint = boneTransform.GetComponent<CharacterJoint>();
            limb.RagdollMotion = boneTransform.GetComponent<RagdollMotion>();

            if (limb.RagdollMotion != null)
            {
                if (animationboneTransform != null)
                {
                    limb.RagdollMotion.TargetBody = animationboneTransform;
                }
                limb.RagdollMotion.InitalRotation = limb.transform.localRotation;
            }
        }

//ConfigurableJointExtensions.SetupAsCharacterJoint(limb.configurableJoint);
//
    }

}

/// <summary>
/// Ragdoll information on its limb. Contains transform data, parent information and joint data
/// </summary>
[Serializable]

public class RagdollLimb
{
    public HumanBodyBones HumanBoneId;
    public Transform transform;
    public Rigidbody rigidbody;
    public ConfigurableJoint configurableJoint;
    public Rigidbody ParentRigidbody;
    public RagdollMotion RagdollMotion;
    public CharacterJoint characterJoint;
}

[Serializable]
public class RagdolLimbState
{
    [Range(0f, 1f)] public float Weight;
    public float MaxAngularDriveX = 720f;
    public float MaxAngularDriveYZ = 720f;
    public ConfigurableJointMotion ConfigurableJointMotion;
   
    public bool isConnected = true;
    /// <summary>
    /// Updates the the state of the ragdoll limb. 
    /// Used for updating the weight of springyness that should be apply to the bone.
    /// Used for updatting r
    /// </summary>
    /// <param name="ragdollLimb"></param>
    public void UpdateState(RagdollLimb ragdollLimb)
    {

        if (ragdollLimb.configurableJoint == null)
            return;

        ragdollLimb.configurableJoint.angularXMotion = ConfigurableJointMotion;
        ragdollLimb.configurableJoint.angularYMotion = ConfigurableJointMotion;
        ragdollLimb.configurableJoint.angularZMotion = ConfigurableJointMotion;

        if (!isConnected)
        {
            ragdollLimb.configurableJoint.connectedBody = null;
            ragdollLimb.configurableJoint.xMotion = ConfigurableJointMotion.Free;
            ragdollLimb.configurableJoint.yMotion = ConfigurableJointMotion.Free;
            ragdollLimb.configurableJoint.zMotion = ConfigurableJointMotion.Free;

            ragdollLimb.transform.parent = null;

            if (ragdollLimb.characterJoint != null)
            {
                ragdollLimb.characterJoint.connectedBody = null;
                ragdollLimb.characterJoint.connectedMassScale = 1;
            }

         
            ragdollLimb.RagdollMotion.enabled = false;
        }
        else
        {
           // ragdollLimb.configurableJoint.xMotion = ConfigurableJointMotion;
           // ragdollLimb.configurableJoint.yMotion = ConfigurableJointMotion;
           // ragdollLimb.configurableJoint.zMotion = ConfigurableJointMotion;


            /*if (ragdollLimb.ParentRigidbody != null)
            {
                ragdollLimb.configurableJoint.connectedBody = ragdollLimb.ParentRigidbody;

                ragdollLimb.transform.parent = ragdollLimb.ParentRigidbody.transform;
            }  */
        }
    }

    public void UpdateWeight(RagdollLimb ragdollLimb)
    {
        if (ragdollLimb.configurableJoint == null)
            return;

        float weight = 720f * Weight;

        var xDrive = ragdollLimb.configurableJoint.angularXDrive;
      
      //  xDrive.positionDamper = weight / 2;
        xDrive.positionSpring = weight;

        var YXDrive = ragdollLimb.configurableJoint.angularYZDrive;

       // YXDrive.positionDamper = weight / 2;
        YXDrive.positionSpring = weight;

        var xd = ragdollLimb.configurableJoint.xDrive;
        xd.positionSpring = weight;
        var yd = ragdollLimb.configurableJoint.yDrive;
        yd.positionSpring = weight;

        var zd = ragdollLimb.configurableJoint.zDrive;
        zd.positionSpring = weight;


        ragdollLimb.configurableJoint.angularXDrive = xDrive;
        ragdollLimb.configurableJoint.angularYZDrive = YXDrive;

    }
}

