using System;
using UnityEngine.SceneManagement;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject keybindsMenuUI;
    [SerializeField] private Story storyScript;
    
    public void Play()
    {
        // Hide main menu immediately
        mainMenuUI.SetActive(false);
        
        // Play story sequence first
        if (storyScript != null)
        {
            storyScript.PlayStory(OnStoryComplete);
        }
        else
        {
            // If no story script, start game directly
            OnStoryComplete();
        }
    }
    
    private void OnStoryComplete()
    {
        // Enable player movement after story completes
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Keybinds()
    {
        keybindsMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void Back()
    {
        keybindsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
