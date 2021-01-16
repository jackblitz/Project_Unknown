using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitLocation : MonoBehaviour
{
    public CharacterAimMotor AimMotor;
    public float ShotHeight = 1.7f;
    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = new Vector3(AimMotor.transform.position.x, ShotHeight, AimMotor.transform.position.z);
        ray.direction = AimMotor.Position;

        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.point != Vector3.zero)
        {
           transform.position = hitInfo.point;
           transform.forward = hitInfo.normal;
        }
        else
        {
            transform.position = ray.origin + AimMotor.Position * 5;
            transform.forward = AimMotor.Position;
        }
    }
}
