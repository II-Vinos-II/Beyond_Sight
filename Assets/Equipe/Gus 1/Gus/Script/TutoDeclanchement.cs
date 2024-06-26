using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutoDeclanchement : MonoBehaviour
{
    [Header("Booleans")]

    public bool learn_Move;
    public bool learn_Jump;
    public bool learn_Whistle;
    public bool learn_Crouch;
    public bool learn_Interract;
    public bool alreadyWhisthled = false;
    

    [Header("GameObjects")]
    public TextMeshProUGUI tutoTexts;
    public Image textBG;
    public PlayerController2 player;
    public GameObject MonstreIntro;
    public SalleController salleController;
    public GameObject panneau1;
    public GameObject panneau2;
    public GameObject panneau3;
    public GameObject panneau4;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController2>();
    }

    // Update is called once per frame
    void Update()
    {

        if (learn_Move)
        {
            if(panneau1 != null)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.D))
                {
                    panneau1.GetComponent<Animator>().enabled = false;
                    panneau1.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
        if (learn_Jump)
        {
            if (panneau2 != null)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    panneau2.GetComponent<Rigidbody>().useGravity = true;
                }
            }

        }
        if (learn_Whistle)
        {
            if (!alreadyWhisthled)
            {
                if (panneau3 != null)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        //if (panneau3.GetComponent<Rigidbody>() != null)
                        //    panneau3.GetComponent<Rigidbody>().useGravity = true;
                        if (GetComponent<BoxCollider>() != null)
                            GetComponent<BoxCollider>().enabled = false;
                        player.canJump = false;
                        player.canMove = false;
                        if (panneau3.GetComponent<Animator>() != null)
                        {
                            panneau3.GetComponent<Animator>().Play("PanneauWhistle");
                            salleController.learn_Crouch_Room = true;
                        }
                        MonstreIntro.SetActive(true);
                        if (salleController.vCamManual.GetComponent<Animator>() != null)
                        {
                            salleController.vCamManual.GetComponent<Animator>().Play("IntroMonstreCam");
                        }
                        //player.DisableMove();
                        StartCoroutine(Wait());
                        alreadyWhisthled = true;
                    }
                }
            }

        }
        if (learn_Crouch)
        {
            panneau4.SetActive(true);
            if (panneau4 != null)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    //panneau4.GetComponent<Rigidbody>().useGravity = true;
                    if (panneau3.GetComponent<Animator>() != null)
                    {
                        panneau3.GetComponent<Animator>().Play("PanneauWhistle");
                    }
                }
            }

        }
    }

    public void IlSenVa()
    {
        MonstreIntro.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.isDoingATuto = true;
            player.DisableMove();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(13.4f);
        player.EnableMove();
        MonstreIntro.SetActive(false);
        learn_Whistle = false;
        player.isDoingATuto=false;
        player.canJump = true;
        player.canMove = true;
    }
}
