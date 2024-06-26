using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanneController : MonoBehaviour
{
    public bool canActivate;
    public GameObject fum�e;
    public GameObject fum�eBlocking;

    // Start is called before the first frame update
    void Start()
    {
        fum�e.SetActive(true);
        if(fum�eBlocking != null)
        {
            fum�eBlocking.SetActive(false);
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
        fum�e.SetActive(false);

        if (fum�eBlocking != null)
        {
            fum�eBlocking.SetActive(true);
        }
        yield return new WaitForSeconds(.5f);
        canActivate = true;
    }
}
