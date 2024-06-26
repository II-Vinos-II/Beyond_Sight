using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraHorizontaleControl : MonoBehaviour
{
    PlayerController player;

    [Header ("Cameras")]
    public CinemachineFreeLook vCam;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
