using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Switches : MonoBehaviour
{
    public SalleController salleController;
    public GameObject[] steamvans;
    public bool isPlayerNear;
    public bool isChanging;
    public bool canSwitchCam;

    private Animator switchesAnim;
    private PlayerController2 player;

    [SerializeField] private GameObject canvasTuto;

    private void Start()
    {
        player = FindObjectOfType<PlayerController2>();
        switchesAnim = GetComponent<Animator>();
        canvasTuto = GameObject.Find("TutoText");
    }

    void Update()
    {
        if (InputManager._interactDown && isPlayerNear && (salleController == null || salleController.inSwitchZone) &&
            !isChanging && !player.isTurning && !player.isCrouch)
        {
            isChanging = true;
            GetComponent<Animation>().Play("ChangingAnim");

            player.GetComponent<Animator>().SetTrigger("VanneTrigger");
            player.PlayerToPoint(new Vector3(transform.position.x, player.transform.position.y, player.transform.position.z), 1f);
        }
    }

    public void ChangeSteam()
    {
        if (steamvans.Length != 0)
        {
            for (int i = 0; i < steamvans.Length; i++)
            {
                steamvans[i].GetComponent<SteamVans>().isSteaming = !steamvans[i].GetComponent<SteamVans>().isSteaming;
                steamvans[i].GetComponent<SteamVans>().ChangeState();
            }
        }

        isChanging = false;
        player.EnableMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (player.onKeyboard)
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = "E to Interact";
            }
            else
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = "X to Interact";
            }

            canvasTuto.GetComponent<TextMeshProUGUI>().color = Color.white;

            if (salleController != null) 
            { 
                salleController.inSwitchZone = true; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isPlayerNear = false;
            canvasTuto.GetComponent<TextMeshProUGUI>().color = Color.clear;
            if (salleController != null) { salleController.inSwitchZone = false; }
        }
    }
}