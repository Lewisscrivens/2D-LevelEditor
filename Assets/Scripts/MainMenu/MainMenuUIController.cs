using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour {

    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject SettingsMenu;

    [SerializeField]
    private GameObject GameSettingsMenu;

    [SerializeField]
    private GameObject VideoSettingsMenu;

    // Menu Controller Methods
    public void enableMainMenu()
    {
        MainMenu.SetActive(true);
    }

    public void disableMainMenu()
    {
        MainMenu.SetActive(false);
    }

    public void OpenLevelEditor()
    {
        SceneManager.LoadScene("LevelEditor");// Loads the level editor.
    }

    public void quitGame()
    {
        Application.Quit();// Exits the application.
    }

    // Methods bellow are for enabling and disabling the UI panels in the MainMenu scene.
    public void enableSettingsMenu()
    {
        SettingsMenu.SetActive(true);
    }

    public void disableSettingsMenu()
    {
        SettingsMenu.SetActive(false);
    }

    public void enableGameSettingsMenu()
    {
        GameSettingsMenu.SetActive(true);
    }

    public void disableGameSettingsMenu()
    {
        GameSettingsMenu.SetActive(false);
    }

    public void enableVideoSettingsMenu()
    {
        VideoSettingsMenu.SetActive(true);
    }

    public void disableVideoSettingsMenu()
    {
        VideoSettingsMenu.SetActive(false);
    }
}
