using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoutonPorte : MonoBehaviour
{
    [SerializeField] private GameObject canvasTuto;
    bool canInterract;
    public GameObject BRUITS;
    public bool interracted = false;
    public Animator animatorLevier;

    // Start is called before the first frame update
    void Start()
    {
        canvasTuto = GameObject.Find("TutoText");
    }

    // Update is called once per frame
    void Update()
    {
        if (canInterract)
        {
            if (InputManager._interactDown)
            {
                interracted = true;
                BoutonWork();
                print("pa touché");
                animatorLevier.Play("Levier");
            }
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInterract = true;
            if (FindObjectOfType<PlayerController2>().onKeyboard)
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = "E to Interact";
            }
            else
            {
                canvasTuto.GetComponent<TextMeshProUGUI>().text = "X to Interact";
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        canInterract = false;
        if (other.transform.CompareTag("Player"))
        {
            canvasTuto.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void BoutonWork()
    {
        
        
        BRUITS.SetActive(true);
    }
}
