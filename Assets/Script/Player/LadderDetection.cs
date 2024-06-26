using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDetection : MonoBehaviour
{
    private PlayerController2 player;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController2>();
        player.debutLadder = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder" && !player.debutClimb && !player.isTurning && !player.debutLadder &&
            player.status != PlayerController2.STATUS.LADDER && player.status != PlayerController2.STATUS.CLIMBING)
        {
            player.debutLadder = true;
            player.DisableMove();
            player.PlayerToPoint(new Vector3(other.transform.parent.parent.GetChild(0).transform.position.x, player.transform.position.y, player.transform.position.z), 0.2f);
        }
    }
}
