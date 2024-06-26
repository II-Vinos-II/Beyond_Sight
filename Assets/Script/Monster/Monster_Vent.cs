using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_Vent : MonoBehaviour
{
    public bool isMonsterHere = false;
    public bool canDoSomething = true;
    public GameObject[] otherVents;
    private Animation animToPlay;
    public AudioClip soundScreamer;
    private AudioSource aS;
    public GameObject scanPlacement;
    public GameObject lights;

    public bool playerHere;


    void Start()
    {
        aS = GetComponent<AudioSource>();
        animToPlay = GetComponent<Animation>();
        MonsterChange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerHere = true;
        }

        if (other.transform.CompareTag("Sound"))
        {
            canDoSomething = true;

            if (!isMonsterHere)
            {
                if (otherVents != null)
                {
                    for (int i = 0; i < otherVents.Length; i++)
                    {
                        if (otherVents[i].transform.GetChild(0).GetComponent<Monster_Vent>().isMonsterHere)
                        {
                            canDoSomething = false;
                        }
                    }
                }

                if (canDoSomething)
                {
                    isMonsterHere = true;
                    if (lights != null)
                    {
                        lights.SetActive(true);
                        //lights.GetComponent<Animator>().Play(1);
                    }
                    animToPlay.Play();
                }
            }
            /*else if (GetComponent<BoxCollider>() != null && isMonsterHere)
            {
                MonsterDeath();
            }*/

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerHere = false;
        }
    }

    public void MonsterChange()
    {
        canDoSomething = false;
        isMonsterHere = false;
        if (lights != null)
            lights.SetActive(false);
    }

    public void PlayMonsterSound()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 10, 0.1f);

        //aS.PlayOneShot(soundScreamer);
    }

    public void MonsterScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 5, 0.1f);

        PlayerWalkSounds.GetInstance().PlayRandom(5);
    }

    public void MonsterDeath()
    {
        Debug.Log("MEURT");
        StartCoroutine(transform.Find("ColliderDeath").GetComponent<TrapController>().Die()); 
        playerHere = false;
    }
}
