using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAL : MonoBehaviour
{
    public PlayerController2 playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController2>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = playerController.transform.position;
    }
}
