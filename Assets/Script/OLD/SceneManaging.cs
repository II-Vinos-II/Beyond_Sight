using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneManaging : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject OptionScreen;
    public bool isPaused;

    private PlayerController2 player;
    private EventSystem eventSystem;

    public GameObject firstPauseButton;
    public GameObject firstOptionButton;

    private void Start()
    {
        player = FindObjectOfType<PlayerController2>();
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.firstSelectedGameObject = firstPauseButton;
        PauseScreen.SetActive(false);
    }

    void Update()
    {

        if (GameObject.Find("TitleScreen"))
        {
            if (GameObject.Find("TitleScreen").GetComponent<StartScreen>().gameStarted)
            {
                Paused();
            }
        }
        else
        {
            Paused();

        }
    }

    public void Paused()
    {
        if (InputManager._menuDown)
        {
            if (isPaused)
            {
                if (OptionScreen)
                {
                    OptionScreen.SetActive(false);
                    eventSystem.firstSelectedGameObject = firstPauseButton;
                }
                else if (!OptionScreen)
                {
                    ResumeButton();
                }
            }
            else
            {
                PauseScreen.SetActive(true);
                player.isDoingATuto = true;
                player.DisableMove();
                player.GetComponent<Animator>().SetFloat("Speed", 0f);
                player.GetComponent<Animator>().SetFloat("Y", 0f);
                isPaused = true;
            }
        }       
    }

    public void ResumeButton()
    {
        PauseScreen.SetActive(false);
        isPaused = false;
        player.isDoingATuto = false;
        player.EnableMove();
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OptionButton()
    {
        OptionScreen.SetActive(true);
        eventSystem.firstSelectedGameObject = firstOptionButton;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
