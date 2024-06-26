using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDetection : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        PointLightScanner.GetInstance().StartScanner(transform.position, 15, .25f, 0f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
}
