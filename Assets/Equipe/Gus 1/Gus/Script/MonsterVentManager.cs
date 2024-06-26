using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterVentManager : MonoBehaviour
{
    static MonsterVentManager instance;
    public List<GameObject> monsters;
    public GameObject firststVentTouched;
    public GameObject secondVentTouched;
    public GameObject thirdVentTouched;

    public bool monster0CanGoOut;
    public bool monster1CanGoOut;
    public bool monster2CanGoOut;

    public static MonsterVentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MonsterVentManager>();
            }
            return instance;
        }     
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MonsterDetection();

    }

    public void MonsterDetection()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            firststVentTouched = monsters[0];
            secondVentTouched = monsters[1];
            thirdVentTouched = monsters[2];
        }
    }
}
