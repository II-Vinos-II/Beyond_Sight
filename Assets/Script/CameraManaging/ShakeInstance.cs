using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeInstance : MonoBehaviour
{
    public static ShakeInstance instance;
    public CinemachineBrain brain;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one ShakeInstance in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        brain = GameObject.Find("MainCamera").GetComponent<CinemachineBrain>();
    }

    public static ShakeInstance GetInstance()
    {
        return instance;
    }

    public void CamShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        StartCoroutine(Noise(shakeIntensity, shakeTiming));
    }

    private IEnumerator Noise(float frequencyGain, float timing)
    {
        CinemachineVirtualCamera cmFreeCam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        cmFreeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        cmFreeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
        yield return new WaitForSeconds(timing);
        cmFreeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cmFreeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;

    }

}
