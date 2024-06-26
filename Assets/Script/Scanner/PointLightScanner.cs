using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PointLightScanner : MonoBehaviour
{
    private static PointLightScanner instance;
    private float waitTime = 0.01f;
    [SerializeField]
    public List<Coroutine> stacktodestroy = new List<Coroutine>();
    public bool blackout;

    public bool randomNoise;
    public int randomCount;

    private void Awake()          
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one PointLightScanner in the scene");
        }
        instance = this;
    }

    public static PointLightScanner GetInstance()
    {
        return instance;
    }

    public void BlackOutTrue()
    {
        blackout = true;
        foreach (Coroutine item in stacktodestroy)
        {
            StopCoroutine(item);
        }
        stacktodestroy.Clear();

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("LightPrefab"))
        {
            Destroy(fooObj);
        }
    }


    public void StartScanner(Vector3 spawnPlace, float size, float speed)
    {
        stacktodestroy.Add(StartCoroutine(Scanner(spawnPlace, size, speed, 0, null)));
    }

    public void StartScanner(Vector3 spawnPlace, float size, float speed, float staySpeed)
    {
        stacktodestroy.Add(StartCoroutine(Scanner(spawnPlace, size, speed, staySpeed, null)));
    }

    public void StartScanner(Vector3 spawnPlace, float size, float speed, GameObject sfx)
    {
        stacktodestroy.Add(StartCoroutine(Scanner(spawnPlace, size, speed, 0, sfx)));
    }

    public void StartScanner(Vector3 spawnPlace, float size, float speed, float staySpeed, GameObject sfx)
    {
        stacktodestroy.Add(StartCoroutine(Scanner(spawnPlace, size, speed, staySpeed, sfx)));
    }

    private IEnumerator Scanner(Vector3 _spawnPlace, float _size, float _speed, float _yspeed, GameObject _sfx)
    {
        Vector3 spawnPlace = _spawnPlace;
        float size = _size;
        float speed = _speed;
        float yspeed = _yspeed;
        GameObject sfx = _sfx;

        //instance
        Light lightclone = Instantiate(Resources.Load<GameObject>("Prefabs/LightScan"), spawnPlace, new Quaternion(0, 0, 0, 0)).GetComponent<Light>();
        GameObject scanfx = lightclone.transform.GetChild(0).gameObject;
        GameObject detectionzone = lightclone.transform.GetChild(1).gameObject;

        if(sfx != null)
        {
            GameObject soundSFX = Instantiate(sfx, spawnPlace, new Quaternion(0, 0, 0, 0));
            soundSFX.transform.SetParent(lightclone.transform.parent);
        }


        //set base size and color
        lightclone.range = 0;
        var main = scanfx.transform.GetComponent<ParticleSystem>().main;
        main.startSize = size / 2;
        main.startLifetime = speed;
        detectionzone.transform.localScale = Vector3.zero * 0f;

        //size up
        for (float i = 0; i < speed; i += waitTime)
        {
            lightclone.range += size * waitTime / speed;
            detectionzone.transform.localScale += Vector3.one * size / 2 * waitTime / speed;
            yield return new WaitForSeconds(waitTime);
        }

        //set up new sizes + stay size
        lightclone.range = size;
        yield return new WaitForSeconds(yspeed);

        //fade out
        for (float i = 0; i < speed; i += waitTime)
        {
            lightclone.color -= new Color(1f, 1f, 1f, 0f) * waitTime / speed;
            detectionzone.transform.localScale -= Vector3.one * size / 2 * waitTime / speed;
            yield return new WaitForSeconds(waitTime);
        }

        Destroy(lightclone.transform.gameObject);


        if (randomNoise && randomCount == 0)
        {
            //play monster sound au loin

        }
    }
}
