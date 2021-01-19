using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointHolder : MonoBehaviour
{
    public WayPoint[] WayPoints;
    void Start()
    {
        WayPoints = GetComponentsInChildren<WayPoint>();
    }

    private void OnDrawGizmos()
    {
        WayPoints = GetComponentsInChildren<WayPoint>();

        if (WayPoints.Length > 0)
        {
            Vector3 startPosition = WayPoints[0].transform.position;
            Vector3 previousPosition = startPosition;

            foreach (WayPoint wayPoint in WayPoints)
            {
                Gizmos.DrawWireSphere(wayPoint.transform.position, .3f);
                Gizmos.DrawLine(previousPosition, wayPoint.transform.position);
                previousPosition = wayPoint.transform.position;
            }

            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(WayPoints.Length.ToString());
    }
}
