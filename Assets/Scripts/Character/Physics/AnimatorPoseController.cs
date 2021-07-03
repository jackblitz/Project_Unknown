using System.Text;
using UnityEngine;
public class AnimatorPoseController : MonoBehaviour
{
    struct AnimationController
    {
        public const string SPEED_ID = "Speed";
        public const string HORIZONTAL_DIRECTION_ID = "Horizontal_Direction";
        public const string VERTICAL_DIRECTION_ID = "Vertical_Direction";
        public const string ANGLE_ID = "Angle";
        public const string AIMING_ID = "IsAiming";

        public struct Layers
        {
            public const string BASE_LAYER = "Base Layer";
        }

        public struct Exploration
        {
            public const string STATE_NAME = "Exploration Locomotion";
            public const string BASE_LOCOMOTION = "Locomotion";
            public const string LOCOMOTION_PIVOT_L = "LocomotionPivotL";
            public const string LOCOMOTION_PIVOT_R = "LocomotionPivotR";
            public const string IDLE_PIVOT_L = "IdlePivotL";
            public const string IDLE_PIVOT_R = "IdlePivotR";

            public static int BASE_LOCOMOTION_ID;
            public static int LOCOMOTION_PIVOT_L_ID;
            public static int LOCOMOTION_PIVOT_R_ID;
            public static int IDLE_PIVOT_L_ID;
            public static int IDLE_PIVOT_R_ID;
        }

    }


    public Vector3 RootMotion = Vector3.zero;
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        OnSetupAnimatorID();

    }

    private void OnSetupAnimatorID()
    {
        string[] LocomotionLayer = new string[] { AnimationController.Layers.BASE_LAYER, AnimationController.Exploration.STATE_NAME };

        AnimationController.Exploration.BASE_LOCOMOTION_ID = Animator.StringToHash(GetIDName(LocomotionLayer, AnimationController.Exploration.BASE_LOCOMOTION));
        AnimationController.Exploration.LOCOMOTION_PIVOT_L_ID = Animator.StringToHash(GetIDName(LocomotionLayer, AnimationController.Exploration.LOCOMOTION_PIVOT_L));
        AnimationController.Exploration.LOCOMOTION_PIVOT_R_ID = Animator.StringToHash(GetIDName(LocomotionLayer, AnimationController.Exploration.LOCOMOTION_PIVOT_R));
        AnimationController.Exploration.IDLE_PIVOT_L_ID = Animator.StringToHash(GetIDName(LocomotionLayer, AnimationController.Exploration.IDLE_PIVOT_L));
        AnimationController.Exploration.IDLE_PIVOT_R_ID = Animator.StringToHash(GetIDName(LocomotionLayer, AnimationController.Exploration.IDLE_PIVOT_R));
    }

    private string GetIDName(string[] layers, string animationName)
    {
        StringBuilder strBild = new StringBuilder();

        foreach(string layer in layers)
        {
            strBild.Append(layer);
            strBild.Append(".");
        }

        strBild.Append(animationName);

        return strBild.ToString();
    }

    /// <summary>
    /// Set the speed you wish the player to go.
    /// This is not the speed the player will go
    /// </summary>
    /// <param name="speed">Speed you want the player to go</param>
    public void SetSpeed(float speed)
    {
        Animator.SetFloat(AnimationController.SPEED_ID, speed);
    }

    /// <summary>
    /// Sets the angle in which you intend to go
    /// </summary>
    /// <param name="angle">Angle to go to</param>
    public void SetAngle(float angle)
    {
        Animator.SetFloat(AnimationController.ANGLE_ID, angle);
    }

    /// <summary>
    /// Set the direction of the player
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector3 direction)
    {
        Animator.SetFloat(AnimationController.HORIZONTAL_DIRECTION_ID, direction.x);
        Animator.SetFloat(AnimationController.VERTICAL_DIRECTION_ID, direction.z);
    }

    /// <summary>
    /// 
    /// </summary>

    private void OnAnimatorMove()
    {
        RootMotion += Animator.deltaPosition;
    }

}

