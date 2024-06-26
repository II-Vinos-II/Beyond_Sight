using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Arrival : Monster_Pursuit
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            M_StartPursuit();
        }
    }
}