using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SalleController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] CameraHorizontaleControl camCtrl;
    [SerializeField] PlayerController2 player;
    [SerializeField] List<Switches> switchCam;

    [Header("Floats")]
    public float offset;

    [Header("Booleans")]
    public bool inSwitchZone;

    public bool playerInHorizontale;
    public bool playerInVerticale;
    public bool playerInManual;
    
    public bool isHorizontale;
    public bool isVerticale;
    public bool isManual;

    public bool canBeHorizontale;
    public bool canBeVerticale;
    public bool canBeManual;

    public bool learn_Move_Room;
    public bool learn_Jump_Room;
    public bool learn_Whistle_Room;
    public bool learn_Crouch_Room;
    public bool learn_Interract_Room;

    [Header("GameObjects")]
    public TutoDeclanchement tutoDeclanchement;

    [Header("Cameras")]
    public CinemachineFreeLook vCamSwitch;
    public CinemachineVirtualCamera vCamHorizontale;
    public CinemachineFreeLook vCamVerticale;
    public CinemachineVirtualCamera vCamManual;


    // Start is called before the first frame update
    void Start()
    {
        camCtrl = FindObjectOfType<CameraHorizontaleControl>();
        player = FindObjectOfType<PlayerController2>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < switchCam.Count; i++)
        {
            if (inSwitchZone)
            {
                if (switchCam[i].canSwitchCam)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(SwitchCamControl());
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
                return;
        }


        if (isHorizontale)
        {
            Horizontale();
        }

        if (isVerticale)
        {
            Verticale();
        }

        if (isManual)
        {
            //Manual();
        }
    }

    public void Horizontale()
    {
        if(vCamHorizontale != null)
            vCamHorizontale.transform.position = new Vector3(player.transform.position.x , vCamHorizontale.transform.position.y + 6, vCamHorizontale.transform.position.z);
    }


    public void Verticale()
    {
        if(vCamVerticale != null)
        {
            vCamVerticale.transform.position = new Vector3(vCamVerticale.transform.position.x, player.transform.position.y + 6, vCamVerticale.transform.position.z);

            
            ////// FAIRE UN CAMERA ADAPTATIVE EN FONCTION DE LA HAUTEUR DU PERSO DANS LA SALLE
           
            
            /*if(vCamVerticale.transform.position.y <= 28f)             
            {
            }
            else if(vCamVerticale.transform.position.y > 28 && vCamVerticale.transform.position.y <= 38)
            {
                vCamVerticale.transform.position = new Vector3(vCamVerticale.transform.position.x, player.transform.position.y + 5, vCamVerticale.transform.position.z);
            }
            else if (vCamVerticale.transform.position.y > 38)
            {
                vCamVerticale.transform.position = new Vector3(vCamVerticale.transform.position.x, player.transform.position.y, vCamVerticale.transform.position.z);
            }*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (learn_Move_Room)
                tutoDeclanchement.learn_Move = true;

            if (learn_Jump_Room)
                tutoDeclanchement.learn_Jump = true;

            if (learn_Whistle_Room)
                tutoDeclanchement.learn_Whistle = true;

            if (learn_Crouch_Room)
                tutoDeclanchement.learn_Crouch = true;

            if (learn_Interract_Room)
                tutoDeclanchement.learn_Interract = true;


            if (canBeHorizontale)
            {
                vCamHorizontale.Priority = 10;
                isHorizontale = true;
                isVerticale = false;
            }

            if (canBeVerticale)
            {
                vCamVerticale.Priority = 10;
                isVerticale = true;
                isHorizontale = false;
            }

            if (canBeManual)
            {
                vCamManual.Priority = 10;
                isVerticale = false;
                isHorizontale = false;
                isManual = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (learn_Move_Room)
                tutoDeclanchement.learn_Move = true;

            if (learn_Jump_Room)
                tutoDeclanchement.learn_Jump = true;

            if (learn_Whistle_Room)
                tutoDeclanchement.learn_Whistle = true;

            if (learn_Crouch_Room)
                tutoDeclanchement.learn_Crouch = true;

            if (learn_Interract_Room)
                tutoDeclanchement.learn_Interract = true;


            if (canBeHorizontale)
            {
                vCamHorizontale.Priority = 10;
                isHorizontale = true;
                isVerticale = false;
            }

            if (canBeVerticale)
            {
                vCamVerticale.Priority = 10;
                isVerticale = true;
                isHorizontale = false;
            }

            if (canBeManual)
            {
                vCamManual.Priority = 10;
                isVerticale = false;
                isHorizontale = false;
                isManual = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (learn_Move_Room)
                tutoDeclanchement.learn_Move = false;

            if (learn_Jump_Room)
                tutoDeclanchement.learn_Jump = false;

            if (learn_Whistle_Room)
                tutoDeclanchement.learn_Whistle = false;

            if (learn_Crouch_Room)
                tutoDeclanchement.learn_Crouch = false;

            if (learn_Interract_Room)
                tutoDeclanchement.learn_Interract = false;

            if (isHorizontale)
            {
                isHorizontale = false;
                vCamHorizontale.Priority = 0;
            }

            if(isVerticale)
            {
                isVerticale = false;
                vCamVerticale.Priority = 0;
            }

            if (isManual)
            {
                isManual = false;
                vCamManual.Priority = 0;
            }
        }
    }

    IEnumerator SwitchCamControl()
    {
        if (switchCam != null)
        {
            vCamSwitch.Priority = 20;
            vCamManual.Priority = 0;
            yield return new WaitForSeconds(3f);
            vCamManual.Priority = 10;
            vCamSwitch.Priority = 0;
        }
        else
        {
            vCamManual.Priority = 10;
            vCamSwitch.Priority = 0;
        }
    }
}
