using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapController : MonoBehaviour
{
    PlayerController2 playerController;
    public GameObject spawn;
    public Animator dieScreen;
    public M_Pursuit monsterPursuit;
    public M_Pursuit2 monsterPursuit2;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController2>();    
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        playerController.DisableMove();
        dieScreen.Play("Mort_In");
        yield return new WaitForSeconds(1.1f);
        playerController.transform.position = spawn.transform.position;
        yield return new WaitForSeconds(1.1f);
        dieScreen.Play("Mort_Out");
        playerController.EnableMove();
        if (monsterPursuit != null)
        {
            monsterPursuit.gameObject.SetActive(false);
            monsterPursuit.m_Animator.SetBool("2ePhases",false);
        }
        if(monsterPursuit2 != null)
        {
            monsterPursuit2.gameObject.SetActive(false);
        }
    }
}