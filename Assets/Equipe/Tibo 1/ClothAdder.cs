using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothAdder : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SkinnedMeshRenderer>().bones = target.GetComponent<SkinnedMeshRenderer>().bones;

        gameObject.GetComponent<SkinnedMeshRenderer>().rootBone = target.GetComponent<SkinnedMeshRenderer>().rootBone;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
