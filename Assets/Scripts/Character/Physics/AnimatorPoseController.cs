using UnityEngine;
public class AnimatorPoseController : MonoBehaviour
{
    public Vector3 RootMotion = Vector3.zero;
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void setSpeed(float speed)
    {
        Animator.SetFloat("Speed", speed);
    }

    private void OnAnimatorMove()
    {
        RootMotion += Animator.deltaPosition;
    }

}

