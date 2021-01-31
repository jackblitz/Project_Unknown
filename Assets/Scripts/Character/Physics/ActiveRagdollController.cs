using System;
using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{
    private Animator animator;
    private FixedJoint fixedJoint;

    public Rigidbody CharacterRigidbody;

    public Animator StaticAnimator;

    [Header("Hip settings")]
    [SerializeField] public RagdolLimbState HipState = new RagdolLimbState();
    [SerializeField] public RagdollLimb[] Hip;

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

        foreach (RagdollLimb ragdollLimb in Hip)
        {
            InitRagdoll(ragdollLimb);
        }

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

        foreach (RagdollLimb ragdollLimb in Hip)
        {
            HipState.UpdateWeight(ragdollLimb);
        }

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
           // RightLegLimbState.UpdateState(ragdollLimb);
           // RightLegLimbState.UpdateWeight(ragdollLimb);
        }

        foreach (RagdollLimb ragdollLimb in LeftLegLimbs)
        {
            LeftLegLimbState.UpdateState(ragdollLimb);
            LeftLegLimbState.UpdateWeight(ragdollLimb);
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


            if (ragdollLimb.ParentRigidbody != null)
            {
                ragdollLimb.configurableJoint.connectedBody = null;
            }
        }
        else
        {
           // ragdollLimb.configurableJoint.xMotion = ConfigurableJointMotion;
           // ragdollLimb.configurableJoint.yMotion = ConfigurableJointMotion;
           // ragdollLimb.configurableJoint.zMotion = ConfigurableJointMotion;


            if (ragdollLimb.ParentRigidbody != null)
            {
                ragdollLimb.configurableJoint.connectedBody = ragdollLimb.ParentRigidbody;

                ragdollLimb.transform.parent = ragdollLimb.ParentRigidbody.transform;
            }  
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

