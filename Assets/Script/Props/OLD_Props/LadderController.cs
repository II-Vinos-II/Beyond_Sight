using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public bool inLadder;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LadderDetection")
        {
            inLadder = true;
            player.ladder_Controller = gameObject.GetComponent<LadderController>();
            player.canDetectGround = false;
        }
    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LadderDetection")
        {
            inLadder = false;
            if (player.ladder_Controller)
            {
                player.ladder_Controller = null;
            }
            player.canDetectGround = true;
        }
    }
}
