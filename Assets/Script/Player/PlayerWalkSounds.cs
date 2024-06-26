using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerWalkSounds : MonoBehaviour
{
    public static PlayerWalkSounds instance;

    [System.Serializable]
    public class WalkSFX
    {
        public int layerNumber;
        public AudioClip[] walkAC;
    }

    public WalkSFX[] walkSounds;

    public AudioClip fallSound;
    public AudioClip soundOnce;
    private AudioSource aS;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one PlayerWalkSounds in the scene");
        }
        instance = this;
    }

    public static PlayerWalkSounds GetInstance()
    {
        return instance;
    }

    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void PlayWalkSound()
    {
        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, 0.6f) && hitGround.transform.gameObject != null)
        {
            for (int i = 0; i < walkSounds.Length; i++)
            {
                if (hitGround.transform.gameObject.layer == walkSounds[i].layerNumber)
                {
                    PlayRandom(i);
                    break;
                }
            }
        }
    }

    public void PlayClimbSound()
    {
        RaycastHit hitGround;
        Debug.DrawRay(new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), transform.TransformDirection(Vector3.forward * 2f), Color.red, 10f);
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 0.5f), transform.TransformDirection(-Vector3.forward * 1f), Color.blue, 10f);
        LayerMask layerexclude = FindObjectOfType<PlayerController2>().layerExclude;

        if ((Physics.Raycast(new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), transform.TransformDirection(Vector3.forward), out hitGround, 2f, layerexclude) ||
            Physics.Raycast(new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), transform.TransformDirection(-Vector3.forward), out hitGround, 2f, layerexclude)) &&
            hitGround.transform.gameObject != null)
        {

            for (int i = 0; i < walkSounds.Length; i++)
            {
                if (hitGround.transform.gameObject.layer == walkSounds[i].layerNumber)
                {
                    PlayRandom(i);
                    break;
                }
            }
        }

    }

    public void PlayFallSounds()
    {
        RaycastHit hitGround;       
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, 0.6f) && hitGround.transform.gameObject != null)
        {
            for (int i = 0; i < walkSounds.Length; i++)
            {
                if (hitGround.transform.gameObject.layer == walkSounds[i].layerNumber)
                {
                    PlayRandom(i);
                    PlayRandom(i);
                    PlayRandom(i);
                    aS.PlayOneShot(fallSound);
                    break;
                }
            }
        }      
    }

    public void PlaySoundOnce()
    {
        if (soundOnce != null)
        {
            aS.PlayOneShot(soundOnce);
        }
    }

    public void PlayRandom(int soundID)
    {
        //Debug.Log(walkSounds[soundID]);

        aS.PlayOneShot(walkSounds[soundID].walkAC[Random.Range(0, walkSounds[soundID].walkAC.Length)]);
    }
}
