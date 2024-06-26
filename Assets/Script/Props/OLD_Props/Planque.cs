using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planque : MonoBehaviour
{
    public PlayerController playerController;
    public BoxCollider PlankZone;
    public Transform PlankPointIn;
    public Transform PlankPointOut;
    public bool isPlanked;
    private bool canPlank;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton3) && isPlanked)
        {
            GoOutPlank();
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton3) && canPlank)
        {
            GoInPlank();
        }
    }

    public void GoInPlank()
    {

        isPlanked = true;
        playerController.transform.position = PlankPointIn.position;
        playerController.canMove = false;

    }

    public void GoOutPlank()
    {
        playerController.transform.position = PlankPointOut.position;
        playerController.canMove = true;
        isPlanked = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canPlank = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            canPlank = false;
    }


    IEnumerator Delay()
    {
        GoInPlank();
        yield return new WaitForSeconds(1);
    }

}