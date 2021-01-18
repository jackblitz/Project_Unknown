using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(NavMeshAgent))]
public class NavMeshEditor : Editor
{
    private void OnSceneGUI()
    {
        NavMeshAgent navMesh = (NavMeshAgent)target;


        Handles.color = Color.green;
        Handles.DrawLine(navMesh.gameObject.transform.position, navMesh.gameObject.transform.position + navMesh.velocity);


        Handles.color = Color.red;
        Handles.DrawLine(navMesh.gameObject.transform.position, navMesh.gameObject.transform.position + navMesh.desiredVelocity);


        Handles.color = Color.black;
        var agentPath = navMesh.path;

        Vector3 prevCorner = navMesh.gameObject.transform.position;

        foreach(var corner in agentPath.corners)
        {
            Handles.DrawLine(prevCorner, corner);
            Handles.DrawWireCube(corner, new Vector3(0.2f, 0.2f, 0.2f));
            prevCorner = corner;
        }
    }
}
