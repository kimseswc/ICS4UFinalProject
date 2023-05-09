using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    public GameObject soundBar;

    private bool pauseMenuOnScreen = false;
    private bool startMenuOnScreen = false;
    private bool optionMenuOnScreen = false;
    private GameObject startMenu;
    private GameObject pauseMenu;
    private GameObject optionMenu;
    public static float soundBarValue = 0.5f;
    private int previousMenu = -1; // 0: pauseMenu, 1: startMenu
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        startMenu = GameObject.Find("StartMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        optionMenu = GameObject.Find("OptionMenu");

        pauseMenu.SetActive(false);
        pauseMenuOnScreen = false;
        startMenu.SetActive(true);
        startMenuOnScreen = true;
        optionMenu.SetActive(false);
        optionMenuOnScreen = false;

        SoundBarUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if(!startMenuOnScreen && Input.GetKeyDown("escape"))
        {
            if(optionMenuOnScreen)
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

    public void StartGame()
    {
        startMenu.SetActive(false);
        startMenuOnScreen = false;
    }

    public void QuitGame()
    {
        //Application.Quit();
    }

    public void Pause_Resume()
    {
        pauseMenu.SetActive(false);
        pauseMenuOnScreen = false;
    }

    public void Option_Open()
    {
        if(pauseMenuOnScreen)
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
        if (soundBarValue != 0f)
        {
            soundBarValue -= 0.125f;
            SoundBarUpdate();
        }
    }

    public void SoundDecrease()
    {
        if(soundBarValue != 1f)
        {
            soundBarValue += 0.125f;
            SoundBarUpdate();
        }
    }

    private void SoundBarUpdate()
    {
        soundBar.GetComponent<Image>().fillAmount = soundBarValue;
        Debug.Log(soundBarValue);
    }
}
