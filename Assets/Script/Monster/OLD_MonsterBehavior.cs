using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MonsterBehavior : MonoBehaviour
{
    private Vector3 currentTarget;
    private Coroutine timerCoroutine;
    private NavMeshAgent enemy;

    public TextMeshPro texttest;

    private enum MonsterState {sleep, active, chase}
    MonsterState currentState;

    void Start()
    {
        currentState = MonsterState.sleep;
        currentTarget = this.transform.position;
        enemy = transform.parent.GetComponent<NavMeshAgent>();
    }

    void Update()
    {        
        switch (currentState)
        {
            case MonsterState.sleep:
                texttest.text = "sleep...";
                Debug.Log("state:sleep");
                break;
            case MonsterState.active:
                texttest.text = "actif!";
                Debug.Log("state:active");
                break;
            case MonsterState.chase:
                texttest.text = "CHASE";
                enemy.SetDestination(currentTarget);
                Debug.Log("state:chase");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Sound"))
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            if(currentState == MonsterState.sleep)
            {
                currentState = MonsterState.active;
            }
            else if(currentState == MonsterState.active)
            {
                currentState = MonsterState.chase;
            }

            currentTarget = other.transform.position;
            timerCoroutine = StartCoroutine(StateChangeTimer(2f));
        }
    }

    IEnumerator StateChangeTimer(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        currentState = MonsterState.sleep;
        currentTarget = this.transform.position;
    }
}
