using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused { get; set; }
    public GameObject PauseMenuUI;
    public GameObject ConformationMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
                Resume();
            else
                Pause();
        }
    }

    private void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GamePaused = true;
    }

    public void ResumeButtonOnClick()
    {
        Resume();
    }
    
    public void SettingsButtonOnClick()
    {
        //TODO 
        Debug.Log("Settings menu opened");
    }

    public void QuitButtonOnClick()
    {
        Resume();
        SceneTransitions.LoadScene("Menu", 1);
    }
    
    public void QuitConfirmation()
    {
        PauseMenuUI.SetActive(false);
        ConformationMenuUI.SetActive(true);
    }
    
    public void Menu()
    {
        PauseMenuUI.SetActive(true);
        ConformationMenuUI.SetActive(false);
    }
}
