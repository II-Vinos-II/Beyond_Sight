using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Pursuit2 : MonoBehaviour
{
    [SerializeField] Monster_Pursuit monsterParentScript;
    public Animator m_Animator;
    public Animator m_Animator2;

    // Start is called before the first frame update
    void Start()
    {
        monsterParentScript = GetComponentInParent<Monster_Pursuit>();
        m_Animator2 = GetComponent<Animator>();
        monsterParentScript.M_Fin();
    }
}