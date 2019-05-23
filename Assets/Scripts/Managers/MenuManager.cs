using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuContainer;
    [SerializeField] private GameObject levelSelectionContainer;
    [SerializeField] private GameObject confirmationContainer;
    
    public void MainMenu()
     {
         mainMenuContainer.SetActive(true);
         levelSelectionContainer.SetActive(false);
         confirmationContainer.SetActive(false);
     }
    
    public void LevelSelection()
    {
        mainMenuContainer.SetActive(false);
        levelSelectionContainer.SetActive(true);
        confirmationContainer.SetActive(false);
    }

    public void QuitConfirmation()
    {
        mainMenuContainer.SetActive(false);
        levelSelectionContainer.SetActive(false);
        confirmationContainer.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
