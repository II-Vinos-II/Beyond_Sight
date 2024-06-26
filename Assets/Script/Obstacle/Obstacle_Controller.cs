using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle_Controller : MonoBehaviour
{
    public PlayerController player;
    public bool canCross;
   
    public GameObject pointA;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pointA.transform.position = new Vector3(pointA.transform.position.x, pointA.transform.position.y, pointA.transform.position.z);

    }

    // Update is called once per fram5
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitClimb"))
        {
            canCross = true;
            player.obstacle_Controller = gameObject.GetComponent<Obstacle_Controller>();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HitClimb"))
        {
            if (player.percentageCompleteA >= 1 || player.status == PlayerController.STATUS.MOVING)
                canCross = false;
            else
                return;
        }
    }
}