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
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        SceneManager.sceneLoaded += OnGameLoaded;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void OnGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            VolumeSettings.instance.GetReferences();
            GetGameReferences();
        }
        SceneManager.sceneLoaded -= OnGameLoaded;
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        HUD.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        HUD.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void GoToControls()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void GoToSettings()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        controlsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        AudioManager.instance.PlayButtonSFX(3);
        SceneManager.sceneLoaded += OnMainMenuLoaded;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnMainMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            VolumeSettings.instance.GetReferences();
            GetMenuReferences();
        }

        SceneManager.sceneLoaded -= OnMainMenuLoaded;
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        Application.Quit();
    }

    public void GetMenuReferences()
    {
        homeScreen = GameObject.Find("HomeScreen");

        controlsScreen.SetActive(true);
        controlsScreen = GameObject.Find("ControlsScreen");
        controlsScreen.SetActive(false);

        settingsScreen.SetActive(true);
        settingsScreen = GameObject.Find("SettingsScreen");
        settingsScreen.SetActive(false);
    }

    public void GetGameReferences()
    {
        controlsScreen = GameObject.Find("ControlsScreen");
        controlsScreen.SetActive(false);

        settingsScreen = GameObject.Find("SettingsScreen");
        settingsScreen.SetActive(false);

        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);

        HUD = GameObject.Find("HUD");
    }
}
