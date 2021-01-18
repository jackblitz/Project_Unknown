using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public class VisibleObject
    {
        public VisibleObject(float distance, GameObject target, Vector3 direction)
        {
            Distance = distance;
            Object = target;
            Direction = direction;
        }

        public float Distance;
        public GameObject Object;
        public Vector3 Direction;
    }
    public float ViewRadius;

    [Range(0,360)]
    public float ViewAngle;

    public LayerMask TargetMask;
    public LayerMask ObjectMask;

    public List<VisibleObject> VisibleObjects = new List<VisibleObject>();

    public Transform Direction;

    public void Start()
    {
        //StartCoroutine("FindTargetsWithDelay", .1f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void Update()
    {
       // FindVisibleTargets();
    }

    public void FindVisibleTargets()
    {
        VisibleObjects.Clear();
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);

        for(int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 playerDirection = (Direction.position - transform.position).normalized;

            if (Vector3.Angle(playerDirection, dirToTarget) < ViewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                float AimDistance = Vector3.Distance(Direction.position, target.position);
                //If there are no obstacles in the way, then we can see the target
                if (!Physics.Raycast(transform.position, dirToTarget, distance, ObjectMask))
                {
                    VisibleObjects.Add(new VisibleObject(AimDistance, target.gameObject, dirToTarget));

                    VisibleObjects.Sort(new DistanceComparer());
                }
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += Direction.rotation.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public class DistanceComparer : IComparer<VisibleObject>
    {
        public int Compare(VisibleObject x, VisibleObject y)
        {
            return x.Distance.CompareTo(y.Distance);
        }
    }
}
