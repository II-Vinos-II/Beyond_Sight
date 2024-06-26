using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Pursuit : MonoBehaviour
{
    [SerializeField] Monster_Pursuit monsterParentScript;
    public Animator m_Animator;
    public Animator m_Animator2;
    public BoutonPorte BOUTON;
    public AudioSource aS;
    public GameObject END;

    // Start is called before the first
    // update
    void Start()
    {
        monsterParentScript = GetComponentInParent<Monster_Pursuit>();
        m_Animator = GetComponent<Animator>(); 
        monsterParentScript.M_Run();
    }

    public void ENDING()
    {
        if (BOUTON.interracted)
        {
            m_Animator.SetBool("good", true);
            aS.Play();
        }
        else
        {
            m_Animator.SetBool("bad", true);
        }
    }

    public void GOODENDING()
    {
        END.SetActive(true);
        aS.gameObject.SetActive(false);
    }
}


