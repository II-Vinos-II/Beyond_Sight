using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class StencilScanner : MonoBehaviour
{
    private static StencilScanner instance;

    private float waitTime = 0.01f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one StencilScanner in the scene");
        }
        instance = this;
    }

    public static StencilScanner GetInstance()
    {
        return instance;
    }

    public void StartScanner(Vector3 spawnPlace, float size, float speed, bool sphere)
    {
        Vector3 _spawnPlace = spawnPlace;
        float _size = size;
        float _speed = speed;
        bool _sphere = sphere;

        StartCoroutine(Scanner(_spawnPlace, _size, _speed, _sphere));
    }

    private IEnumerator Scanner(Vector3 _spawnPlace, float _size, float _speed, bool _sphere)
    {
        Vector3 spawnPlace = _spawnPlace;
        float size = _size;
        float speed = _speed;
        bool sphere = _sphere;


        //instance
        GameObject scanclone;
        if (sphere)
        {
            scanclone = Instantiate(Resources.Load<GameObject>("Prefabs/StencilScanSphere"), spawnPlace, new Quaternion(0, 0, 0, 0));
        }
        else
        {
            scanclone = Instantiate(Resources.Load<GameObject>("Prefabs/StencilScanPlane"), spawnPlace += new Vector3(0, 0, -3f), new Quaternion(0,0,0,0));
        }


        //set var
        GameObject stencil2 = scanclone.transform.GetChild(0).gameObject;
        GameObject stencil1 = scanclone.transform.GetChild(1).gameObject;
        GameObject shadow = scanclone.transform.GetChild(2).gameObject;
        GameObject scanfx = scanclone.transform.GetChild(3).gameObject;

        //set base size and color
        stencil2.transform.localScale = Vector3.zero;
        stencil1.transform.localScale = Vector3.zero;
        shadow.transform.localScale = Vector3.zero;
        shadow.GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 0f, 0f);
        scanfx.transform.localScale = Vector3.zero;

        StartCoroutine(StartEcho(scanfx, size, speed, scanclone.transform));
        /*var main = scanfx.transform.GetComponent<ParticleSystem>().main;
        main.startSize = size;
        main.startLifetime = speed;*/

        //expand first orb
        for (float i = 0; i < speed; i += waitTime)
        {
            stencil2.transform.localScale += Vector3.one * size * waitTime / speed;
            yield return new WaitForSeconds(waitTime);
        }

        //set up new sizes
        stencil2.transform.localScale = Vector3.one * (size - size / 100f);
        stencil1.transform.localScale = Vector3.one * (size - size / 100f);
        shadow.transform.localScale = Vector3.one * size;

        //float haha = waitTime;
        float haha = 100f;

        //fade out
        for (float i = 0; i < speed; i += waitTime)
        {
            //size down first orb
            if (stencil2.transform.localScale.x > 0)
            {
                stencil2.transform.localScale -= Vector3.one * size * (waitTime * 6f) / speed;
            }
            else
            {
                stencil2.transform.localScale = Vector3.zero;
            }

            //expand slowly second orb and black orb [A REFAIRE]
            //stencil1.transform.localScale += Vector3.one * size * haha / speed;
            stencil1.transform.localScale += Vector3.one * size / haha;
            //shadow.transform.localScale += Vector3.one * size * haha / speed;
            shadow.transform.localScale += Vector3.one * size / haha;
            //haha += size * 2f;
            haha += size * 2f;

            //fade out
            shadow.GetComponent<MeshRenderer>().material.color += new Color(0f, 0f, 0f, 1 * waitTime / speed);
            yield return new WaitForSeconds(waitTime);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(scanclone);
    }

    IEnumerator StartEcho(GameObject echo, float echosize, float echospeed, Transform parent)
    {
        float sizeModificator = 2;
        float speedModificator = 2;

        for (float i = 0; i < 4; i += 1)
        {
            GameObject echoclone = Instantiate(echo, parent.position, parent.rotation, parent);
            StartCoroutine(EchoEffect(echoclone, echosize * sizeModificator / 10, echospeed * speedModificator / 10));

            yield return new WaitForSeconds(echospeed / 10);
            sizeModificator += 2;
            speedModificator += 1;
        }
    }

    IEnumerator EchoEffect(GameObject echo, float echosize, float echospeed)
    {
        for (float i = 0; i < echospeed; i += waitTime)
        {
            echo.transform.localScale += new Vector3(1, 1, 0.1f) * echosize * waitTime / echospeed;
            yield return new WaitForSeconds(waitTime);
        }

        for (float i = 0; i < echospeed / 5; i += waitTime)
        {
            echo.transform.localScale += Vector3.one * echosize * waitTime / echospeed / 2;
            echo.GetComponent<MeshRenderer>().material.color -= new Color(0f, 0f, 0f, 1 * waitTime / echospeed / 5);
            yield return new WaitForSeconds(waitTime);
        }

        //Destroy(echo);
    }

}
