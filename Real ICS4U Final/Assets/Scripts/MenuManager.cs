using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    private bool pauseMenuOnScreen = false;
    private bool startMenuOnScreen = false;
    private bool optionMenuOnScreen = false;
    private GameObject startMenu;
    private GameObject pauseMenu;
    private GameObject optionMenu;
    private int previousMenu = -1; // 0: pauseMenu, 1: startMenu
    // Start is called before the first frame update
    void Start()
    {
        startMenu = GameObject.Find("StartMenu");
        pauseMenu = GameObject.Find("PauseMenu");
        optionMenu = GameObject.Find("OptionMenu");

        startMenu.SetActive(true);
        startMenuOnScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape"))
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
}
