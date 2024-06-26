using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlissadeController : MonoBehaviour
{
    PlayerController player;
    public bool isGlissing;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.canMove = false;
            player.glissade_Controller = this;
            isGlissing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.canMove = true;
            player.glissade_Controller = null;
            player.status = PlayerController.STATUS.MOVING;
            isGlissing = false;
        }
    }
}
