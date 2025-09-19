using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Sensitivity")]
    public Slider sensSlider;
    public float sensFromSlider;

    private PrimaryUIState primaryState;
    private SecondaryUIState secondaryState;

    [Header("Primary Screens")]
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject missionScreen;
    [SerializeField] private GameObject HUD;

    [Header("Secondary Screens")]
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;

    private Dictionary<PrimaryUIState, GameObject> primaryScreens;
    private Dictionary<SecondaryUIState, GameObject> secondaryScreens;
    public enum PrimaryUIState
    {
        None,
        Home,
        PauseMenu,
        MissionMenu,
        HUD
    }

    public enum SecondaryUIState
    {
        None,
        Settings,
        Controls
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Initialize();
    }


    private void Initialize()
    {
        primaryScreens = new Dictionary<PrimaryUIState, GameObject>
        {
            { PrimaryUIState.Home, homeScreen },
            { PrimaryUIState.PauseMenu, pauseScreen },
            { PrimaryUIState.MissionMenu, missionScreen },
            { PrimaryUIState.HUD, HUD }
        };

        secondaryScreens = new Dictionary<SecondaryUIState, GameObject>
        {
            { SecondaryUIState.Settings, settingsScreen },
            { SecondaryUIState.Controls, controlsScreen }
        };

        LoadSensitivity();
        sensSlider.onValueChanged.AddListener((v) => SensitivityFromSlider());
    }

    void Start()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if ((SceneIndex)currentIndex == SceneIndex.MainMenu)
        {
            SetPrimary(PrimaryUIState.Home);
            SetSecondary(SecondaryUIState.None);
        }
        else
        {
            SetPrimary(PrimaryUIState.HUD);
            SetSecondary(SecondaryUIState.None);
        }
    }

    public void SetPrimary(PrimaryUIState newState)
    {

        if (primaryState == newState && primaryState != PrimaryUIState.None) return;
        primaryState = newState;

        foreach (var kvp in primaryScreens) kvp.Value.SetActive(false);

        if (primaryScreens.TryGetValue(primaryState, out var screen)) screen.SetActive(true);

        SetSecondary(SecondaryUIState.None);
    }

    public void SetSecondary(SecondaryUIState newState)
    {
        if (secondaryState == newState && secondaryState != SecondaryUIState.None) return;
        secondaryState = newState;

        foreach (var kvp in secondaryScreens) kvp.Value.SetActive(false);

        if (secondaryScreens.TryGetValue(secondaryState, out var screen))
            screen.SetActive(true);
    }
    public void OpenControls()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.Controls);
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.Settings);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayOneShot("Button");
        Application.Quit();
    }

    public void PauseGame(PrimaryUIState menuType)
    {
        GameManager.Instance.Pause();

        AudioManager.Instance.PlayOneShot("Button");
        AudioManager.Instance.PauseSFX();
        
        SetPrimary(menuType);
    }

    public void ResumeGame()
    {
        GameManager.Instance.Play();

        AudioManager.Instance.PlayOneShot("Button");
        AudioManager.Instance.ResumeSFX();

        SetPrimary(PrimaryUIState.HUD);
    }

    public void MainMenu()
    {
        GameManager.Instance.LoadScene(SceneIndex.MainMenu);

        AudioManager.Instance.PlayOneShot("Button");
    }

    public void NewGame()
    {
        GameManager.Instance.LoadScene(SceneIndex.Game);

        AudioManager.Instance.PlayOneShot("Button");

    }

    public void SensitivityFromSlider()
    {
        if (sensSlider == null)
        {
            Debug.LogWarning("Sensitivity slider not assigned yet.");
            return;
        }

        sensFromSlider = sensSlider.value;
        PlayerPrefs.SetFloat("sensitivity", sensFromSlider);
    }

    public void LoadSensitivity()
    {
        if (sensSlider == null)
        {
            Debug.LogWarning("Can't load sensitivity: slider not found yet.");
            return;
        }

        if (PlayerPrefs.HasKey("sensitivity"))
        {
            sensFromSlider = PlayerPrefs.GetFloat("sensitivity");
            sensSlider.value = sensFromSlider; // This may trigger OnValueChanged
        }
        else
        {
            sensFromSlider = sensSlider.value;
            PlayerPrefs.SetFloat("sensitivity", sensFromSlider);
        }
    }
}