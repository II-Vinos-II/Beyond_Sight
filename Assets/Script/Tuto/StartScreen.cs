using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject player;
    private Animation anim;
    private bool canStart;
    public Transform startCoordinate;
    public Transform endCoordinate;
    public AudioClip bangSound;
    public GameObject[] soundDoss;

    public bool gameStarted;


    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = startCoordinate.position;
        //player.GetComponent<PlayerController2>().DisableMove();
        anim = GetComponent<Animation>();
        anim.Play("StartMenuAnim");
        canStart = false;
        gameStarted = false;

        for (int i = 0; i < soundDoss.Length; i++)
        {
            soundDoss[i].SetActive(false);

            /*foreach (Transform child in soundDoss[i].transform)
            {
                child.gameObject.SetActive(false);
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController2>().onKeyboard)
        {
            transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "PRESS 'ENTER' TO START";
        }
        else
        {
            transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "PRESS 'A' TO START";
        }

        if (InputManager._startUp && canStart)
        {
            canStart = false;
            anim.Play("EndMenuAnim");
            player.GetComponent<Animator>().SetTrigger("Start");
            StartCoroutine(Cinematic(endCoordinate.position));
        }
    }

    public void CanStart()
    {
        canStart = true;
    }

    IEnumerator Cinematic(Vector3 endPoint)
    {
        //player.GetComponent<AudioSource>().PlayOneShot(bangSound);

        float elapsedTime = 0;
        float waitTime = 3f;
        Vector3 currentPos = player.transform.position;

        while (elapsedTime < waitTime)
        {
            player.transform.position = Vector3.Lerp(currentPos, endPoint, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = endPoint;
        player.GetComponent<Animator>().SetTrigger("Start");
        player.GetComponent<PlayerController2>().EnableMove();
        gameStarted = true;

        for (int i = 0; i < soundDoss.Length; i++)
        {
            soundDoss[i].SetActive(true);

            /*foreach (Transform child in soundDoss[i].transform)
            {
                child.gameObject.SetActive(true);
            }*/
        }
    }
}
