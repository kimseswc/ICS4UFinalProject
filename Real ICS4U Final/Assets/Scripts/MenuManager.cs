using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    public CharacterController2D player;
    public GameObject soundBar;
    public GameObject BGMSoundPlayer; 

    private bool pauseMenuOnScreen = false;
    private bool startMenuOnScreen = false;
    private bool optionMenuOnScreen = false;
    private bool deathMenuOnScreen = false;
    private GameObject startMenu;
    private GameObject pauseMenu;
    private GameObject optionMenu;
    private GameObject deathMenu;
    public AudioSource soundEffectPlayer;
    public static float soundBarValue = 0.5f;
    private int previousMenu = -1; // 0: pauseMenu, 1: startMenu

    void Start()
    {
        gameObject.SetActive(true);
        startMenu = GameObject.Find("StartMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        optionMenu = GameObject.Find("OptionMenu");
        deathMenu = GameObject.Find("DeathMenu");

        pauseMenu.SetActive(false);
        pauseMenuOnScreen = false;
        startMenu.SetActive(true);
        startMenuOnScreen = true;
        optionMenu.SetActive(false);
        optionMenuOnScreen = false;
        deathMenu.SetActive(false);
        deathMenuOnScreen = false;

        SoundBarUpdate();
    }

    void Update()
    {
        if(!startMenuOnScreen && !deathMenuOnScreen && Input.GetKeyDown("escape"))
        {
            SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
            if (optionMenuOnScreen)
            {
                Option_Resume();
            }

            else if (pauseMenuOnScreen)
            {
                pauseMenu.SetActive(false);
                pauseMenuOnScreen = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                pauseMenuOnScreen = true;
            }
        }
    }

    // --- called by buttons in Menu gameObject.
    public void playerDied()
    {
        deathMenu.SetActive(true);
        deathMenuOnScreen = true;
    }

    public void StartGame()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        startMenu.SetActive(false);
        startMenuOnScreen = false;
        deathMenu.SetActive(false);
        deathMenuOnScreen = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause_Resume()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        pauseMenu.SetActive(false);
        pauseMenuOnScreen = false;
    }

    public void Option_Open()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        if (pauseMenuOnScreen)
        {
            pauseMenuOnScreen = false;
            previousMenu = 0; // previous = pauseMenu
            pauseMenu.SetActive(false);
        }
        else if(startMenuOnScreen)
        {
            startMenuOnScreen = false;
            previousMenu = 1; // previous = startMenu
            startMenu.SetActive(false);
        }
        optionMenu.SetActive(true);
        optionMenuOnScreen = true;
    }

    public void Option_Resume()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        optionMenu.SetActive(false);
        optionMenuOnScreen = false;
        if(previousMenu == 0) // previous = pauseMenu
        {
            pauseMenu.SetActive(true);
            pauseMenuOnScreen = true;
        }
        else if(previousMenu == 1) // previous = startMenu
        {
            startMenu.SetActive(true);
            startMenuOnScreen = true;
        }
    }

    public void SoundIncrease()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        if (soundBarValue != 0f)
        {
            soundBarValue -= 0.125f;
            SoundBarUpdate();
        }
    }

    public void SoundDecrease()
    {
        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.buttonClick);
        if (soundBarValue != 1f)
        {
            soundBarValue += 0.125f;
            SoundBarUpdate();
        }
    }
    // ---

    private void SoundBarUpdate()
    {
        soundBar.GetComponent<Image>().fillAmount = soundBarValue;
        BGMSoundPlayer.GetComponent<SoundManager>().BGMplayer.volume = soundBarValue;
        Debug.Log(soundBarValue);
    }
}
