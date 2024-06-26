using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDetectionZone : MonoBehaviour
{
    public LadderController ladderController;
    PlayerController player;
    public bool ladderInReference;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.status == PlayerController.STATUS.CLIMBING)
        {
            ladderController = null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            ladderController = other.gameObject.GetComponent<LadderController>();
            player.canDetectGround = false;
            //ladderInReference = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            if(ladderController != null)
            {
                ladderController = null;
                player.canDetectGround = true;
                //ladderInReference = false;
            }
        }
    }
}
