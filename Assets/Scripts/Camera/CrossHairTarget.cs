using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    public Transform Direction;
    public Transform Position;
    Ray ray;
    RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
       // mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = new Vector3(Position.transform.position.x, Direction.position.y, Position.transform.position.z);
        ray.direction = Direction.transform.forward;

        Physics.Raycast(ray, out hitInfo);

        transform.position = hitInfo.point;
    }
}
