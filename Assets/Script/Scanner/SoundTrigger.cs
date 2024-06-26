using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    private Transform childTransform;

    public float size = 50f;
    public float speed = 1f;
    public float staySpeed = 0f;
    public AudioClip sound;

    private bool ended;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !ended)
        {
            childTransform = transform.GetChild(0);
            PointLightScanner.GetInstance().StartScanner(childTransform.position, size, speed, staySpeed);
            childTransform.gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
            //Destroy(gameObject);
            ended = true;
        }
    }
}
