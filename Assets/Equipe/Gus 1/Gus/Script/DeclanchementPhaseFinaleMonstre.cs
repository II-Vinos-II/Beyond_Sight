using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeclanchementPhaseFinaleMonstre : Monster_Pursuit
{
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.m_Animator.SetBool("2ePhase", true);
            monster.m_Animator2.gameObject.SetActive(true);
            monster.m_Animator.gameObject.SetActive(true);
        }
    }
}