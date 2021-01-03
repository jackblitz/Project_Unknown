using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class VirtualCameraComposer : MonoBehaviour
{
    public float ZoomLevel = 13f;
    public float Angle = 4f;
    public CinemachineVirtualCamera mVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mVirtualCamera.m_Lens.OrthographicSize = ZoomLevel;
        mVirtualCamera.m_Lens.FarClipPlane = ZoomLevel * Angle;
        CinemachineFramingTransposer frame = mVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (frame != null)
        {
            frame.m_CameraDistance = ZoomLevel;
        }
    }
}
