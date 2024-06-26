using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetection : MonoBehaviour
{
    private PlayerController2 player;
    public bool middleDetec;

    void Start()
    {
        player = transform.parent.GetComponent<PlayerController2>();
        player.debutClimb = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crossable" && player.status != PlayerController2.STATUS.CLIMBING)
        {
            player.debutClimb = true;
            player.DisableMove();
            if (!player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Ladder"))
            {
                if (middleDetec)
                {
                    player.PlayerToPoint(new Vector3(other.transform.GetChild(1).transform.position.x, other.transform.GetChild(1).transform.position.y, player.transform.position.z), 0.1f);
                    player.GetComponent<Animator>().SetFloat("ClimbFloat", 1f);
                    Debug.Log("ARG ARG");
                }
                else
                {
                    player.PlayerToPoint(new Vector3(other.transform.GetChild(0).transform.position.x, other.transform.GetChild(0).transform.position.y, player.transform.position.z), 0.1f);
                    player.GetComponent<Animator>().SetFloat("ClimbFloat", 0f);
                    Debug.Log("GRR GRR");
                }

            }

        }
    }
}
