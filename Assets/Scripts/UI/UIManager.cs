using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject settingsScreen;

    public static UIManager instance;

    public Slider sensSlider;
    public float sensFromSlider;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SensitivityFromSlider();
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

    public void SensitivityFromSlider()
    {
        //Debug.Log(sensSlider.value);
        sensFromSlider = sensSlider.value;
    }
}