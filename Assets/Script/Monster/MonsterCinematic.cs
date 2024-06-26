using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterCinematic : MonoBehaviour
{
    public bool dotuto;

    private PlayerController2 player;
    private Animation monsteranim;
    public GameObject sallecam;
    public AudioClip bigBang;
    public AudioClip monsterScream;
    public AudioClip rockFalling;
    public GameObject[] lightsout;

    private bool animStarted;
    private bool hasentered;
    public GameObject scanPlacement;

    void Start()
    {
        player = FindObjectOfType<PlayerController2>();
        monsteranim = GetComponent<Animation>();
    }

    void Update()
    {
        if (dotuto)
        {
            if (GetComponent<TutorialTrigger>().tutoStart && !animStarted)
            {
                PointLightScanner.GetInstance().blackout = true;

                player.GetComponent<Animator>().SetFloat("Speed", 0);
                player.isDoingATuto = true;
                player.DisableMove();

                if (Input.GetKeyDown(KeyCode.A))
                {
                    monsteranim.Play();
                    animStarted = true;
                }
            }

        }
        else
        {
            if (hasentered && !animStarted)
            {
                PointLightScanner.GetInstance().blackout = true;

                player.GetComponent<Animator>().SetFloat("Speed", 0);
                player.isDoingATuto = true;
                player.DisableMove();
                player.EnableRootMotion();

                monsteranim.Play();
                animStarted = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasentered = true;
        }
    }

    public void FinishAnim()
    {
        player.isDoingATuto = false;
        player.EnableMove();
        PointLightScanner.GetInstance().blackout = false;
    }

    public void MonsterScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 15, 0.1f);

        GetComponent<AudioSource>().PlayOneShot(bigBang);
        GetComponent<AudioSource>().PlayOneShot(bigBang);
        sallecam.GetComponent<Animator>().Play("IntroMonstreCam");
    }

    public void StepScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 5, 0.1f);

        PlayerWalkSounds.GetInstance().PlayRandom(5);
    }

    public void ScanIntroMonstre()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 100, 1, 2);

        GetComponent<AudioSource>().PlayOneShot(monsterScream);
    }

    public void RockFallingScan()
    {
        PointLightScanner.GetInstance().StartScanner(scanPlacement.transform.position, 20, 1);

        GetComponent<AudioSource>().PlayOneShot(rockFalling);
        GetComponent<AudioSource>().PlayOneShot(rockFalling);
        GetComponent<AudioSource>().PlayOneShot(rockFalling);
    }
}
