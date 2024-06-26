using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Rendering;

public class LoopingScannerTest : MonoBehaviour
{

    [Header("Param")]
    public float size = 50f;
    public float speed = 1f;
    public float loopTime = 1f;
    public float staySpeed = 0f;

    private float waitTime = 0.01f;

    [Header("Sound")]
    public AudioClip[] waterdrops;
    private AudioSource aS;

    [Header("SFX")]
    public GameObject sfx;

    private float distance;
    private PlayerController2 player;
    private bool canScan;

    private void Start()
    {
        player = FindObjectOfType<PlayerController2>();
        aS = GetComponent<AudioSource>();
        canScan = true;
    }

    public void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance > 70f)
        {
            canScan = true;
        }
        else if (!PointLightScanner.GetInstance().blackout && distance <= 30f && canScan)
        {
            StartCoroutine(WaitForLoop());
            canScan = false;
        }
    }

    IEnumerator WaitForLoop()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        PointLightScanner.GetInstance().StartScanner(transform.position, size, speed, staySpeed, sfx); 
        if (waterdrops.Length > 0)
            aS.PlayOneShot(waterdrops[Random.Range(0, waterdrops.Length)]);

        for (float i = 0; i < speed * 2; i += waitTime)
        {        
            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(0.5f + loopTime);

        if (!PointLightScanner.GetInstance().blackout && distance < 60f)
        {
            StartCoroutine(Loop());
        }
    }

}
