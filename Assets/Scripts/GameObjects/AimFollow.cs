using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{
    public CharacterAimMotor mAimMotor;
    public BulletHitLocation mHitLocation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(mAimMotor.transform.position.x, mHitLocation.ShotHeight, mAimMotor.transform.position.z);


        transform.position = position + mAimMotor.Position * 5;
        transform.forward = mAimMotor.Position;
    }
}
