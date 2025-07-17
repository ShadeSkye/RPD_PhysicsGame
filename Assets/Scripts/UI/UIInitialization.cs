using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitialization : MonoBehaviour
{
    public static UIInitialization instance;

    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        instance = this;

        if(settingsScreen != null)
            settingsScreen.SetActive(true);

        if (controlsScreen != null)
            controlsScreen.SetActive(true);

        if (pauseScreen != null)
            pauseScreen.SetActive(true);
    }

    public void InitializeUI()
    {
        if (settingsScreen != null)
            settingsScreen.SetActive(true);

        if (controlsScreen != null)
            controlsScreen.SetActive(true);

        if (pauseScreen != null)
            pauseScreen.SetActive(true);
    }
}
