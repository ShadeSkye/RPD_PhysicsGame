using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject settingsScreen;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        SceneManager.LoadSceneAsync(1);
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

    public void QuitGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        Application.Quit();
    }
}