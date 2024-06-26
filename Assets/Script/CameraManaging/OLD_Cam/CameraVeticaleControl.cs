using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVeticaleControl : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public CinemachineVirtualCamera vCamMax;
    public CinemachineVirtualCamera vCamMin;
    public CinemachineVirtualCameraBase vCamBase;

    public bool isInMinimum;
    public bool isInMaximum;
    public Vector3 maxY;
    public Vector3 MinY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // if (transform.localPosition.x <= MinY.x)
       // {
       //     print("<MIn");
       //     vCam.Priority = 0;
       //     vCamMax.Priority = 0;
       //     vCamMin.Priority = 10;
       // }
       // else if (transform.localPosition.x >= maxY.x)
       // {
       //     print(">Max");
       //     vCam.Priority = 0;
       //     vCamMin.Priority = 0;
       //     vCamMax.Priority = 10;
       // }
       // else if (transform.localPosition.x <= maxY.x && transform.localPosition.x >= MinY.x)
       // {
       //     print("<>");
       //     vCam.Priority = 10;
       //     vCamMin.Priority = 0;
       //     vCamMax.Priority = 0;
       // }
    }
}
