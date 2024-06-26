using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLerp : MonoBehaviour
{
    public float climbLerpDuraction = 1f;
    public float elapsedTime;
    public Vector3 startPos;
    public Vector3 endPos = new Vector3 (15.95f, 14.07f);
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / climbLerpDuraction;
        transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

    }
}
