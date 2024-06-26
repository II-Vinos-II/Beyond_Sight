using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterDeclanchement : MonoBehaviour
{
    PlayerController player;
    Animator animator;
    public bool ouaiscamarcheenculedetamerelapute = false;

    public GameObject monstre;
    public bool canDieIfWhistle;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ouaiscamarcheenculedetamerelapute)
        {
            animator = GetComponent<Animator>();
            animator.Play("M_Round");
            GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
            player.canMove = false;
        }

        if (canDieIfWhistle)
        {
            if (player.isWithle)
            {
                Debug.Log("TMOR");
            }
        }
    }

    public void Apparition()
    {
        monstre.SetActive(true);
        canDieIfWhistle = true;
        ouaiscamarcheenculedetamerelapute = false;
        player.canMove=true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ouaiscamarcheenculedetamerelapute=true;
        }
    }
}
