using System;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject keybindsMenuUI;
    public void Play()
    {
        playerMovement.enabled = true;
        mainMenuUI.SetActive(false);
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
