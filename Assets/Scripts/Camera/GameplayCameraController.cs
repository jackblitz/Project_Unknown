using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera MainCamera;
    public CinemachineVirtualCamera OverShoulderCamera;


    private CameraState mCameraState;

    public enum CameraState
    {
        MainCamera,
        OverShoulderCamera,
        CutScene,
        Dolly,
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSetCameraState(CameraState cameraState)
    {
        switch (cameraState)
        {
            case CameraState.MainCamera:
                MainCamera.enabled = true;
                OverShoulderCamera.enabled = false;
                break;
            case CameraState.OverShoulderCamera:
                MainCamera.enabled = false;
                OverShoulderCamera.enabled = true;
                break;
            case CameraState.Dolly:
                break;
            case CameraState.CutScene:
                break;
        }

        mCameraState = cameraState;
    }

}
