using UnityEngine;

public class Monster_Pursuit : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController2 player;
    public M_Pursuit monster;

    [Header("Objects")]
    public GameObject arrival;
    public GameObject plaque; 
    public CapsuleCollider m_Collider;
    public GameObject spawn;
   


    [Header("Values")]
    public float m_Speed;

    [Header("Booleans")]
    public bool m_FarFromPlayer;
    public bool m_canStartRunning = false;

    public bool firstRunPhase;
    public bool secondRunPhase;
    public bool thirbRunPhase;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController2>(); 
        m_Collider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void M_StartPursuit()
    {
       // plaque.GetComponent<Animator>().Play("PlaqueQuiTombe");
        monster.gameObject.SetActive(true);
    }

    public void M_Run()
    {
        monster.m_Animator.SetTrigger("Death");
        print("zpeiv");

    }

    public void M_Fin()
    {
        monster.m_Animator2.SetTrigger("Death2");
        monster.m_Animator.SetTrigger("Death2");
        print("zpzrihgzfzpiedcnppipfen");
    }
}