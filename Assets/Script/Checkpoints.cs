using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public List<GameObject> checks;
    public Transform lastTrigger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FindObjectOfType<PlayerController>().transform.position = lastTrigger.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checks.Add(other.gameObject);
            //lastTrigger = checks.Count //other.gameObject.transform;
        }
    }
}
