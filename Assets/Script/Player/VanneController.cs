using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanneController : MonoBehaviour
{
    public bool canActivate;
    public GameObject fumée;
    public GameObject fuméeBlocking;

    // Start is called before the first frame update
    void Start()
    {
        fumée.SetActive(true);
        if(fuméeBlocking != null)
        {
            fuméeBlocking.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                StartCoroutine(WaitToActivate());
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {           
            canActivate = false;
        }
    }

    IEnumerator WaitToActivate()
    {
        canActivate = false;
        fumée.SetActive(false);

        if (fuméeBlocking != null)
        {
            fuméeBlocking.SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        canActivate = true;
    }
}
