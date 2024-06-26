using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachSphere : MonoBehaviour
{
    public PlayerController2 player;

    void Start()
    {
        player = FindObjectOfType<PlayerController2>();
    }

    void Update()
    {
        transform.position = player.transform.position;
    }
}
