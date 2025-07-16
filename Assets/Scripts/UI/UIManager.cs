using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject HUD;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void NewGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        SceneManager.LoadSceneAsync(1);
    }

    public void PauseGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        HUD.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        HUD.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void GoToControls()
    {
        AudioManager.instance.PlayButtonSFX(3);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void GoToSettings()
    {
        AudioManager.instance.PlayButtonSFX(3);
        controlsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        AudioManager.instance.PlayButtonSFX(3);
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlayButtonSFX(3);
        Application.Quit();
    }
}
