using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Drawing;

public class MenuOption : MonoBehaviour
{
    [SerializeField] private AudioMixer aM;
    [SerializeField] private Slider VolGeneral;
    [SerializeField] private Slider VolMusic;
    [SerializeField] private Slider VolSFX;

    [SerializeField] private Slider FontSize;
    [SerializeField] private TextMeshProUGUI[] Texts;

    private float currentFontSize;

    private void Start()
    {
        CloseMenu();
    }

    private void Update()
    {
        if (InputManager._menuDown)
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void SetGeneralVol()
    {
        float volume = VolGeneral.value;
        aM.SetFloat("Vol_General", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVol()
    {
        float volume = VolMusic.value;
        aM.SetFloat("Vol_Music", Mathf.Log10(volume) * 50f);
    }

    public void SetSFXVol()
    {
        float volume = VolSFX.value;
        aM.SetFloat("Vol_SFX", Mathf.Log10(volume) * 50f);
    }

    public void SetFontSize()
    {
        currentFontSize = FontSize.value;

        foreach (var text in Texts)
        {
            text.fontSize = currentFontSize;
        }
    }
}
