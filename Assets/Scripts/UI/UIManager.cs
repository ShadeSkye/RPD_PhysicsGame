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
        Pause,
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

        //DontDestroyOnLoad(gameObject);

        Initialize();
    }


    private void Initialize()
    {
        primaryScreens = new Dictionary<PrimaryUIState, GameObject>
        {
            { PrimaryUIState.Home, homeScreen },
            { PrimaryUIState.Pause, pauseScreen },
            { PrimaryUIState.HUD, HUD }
        };

        secondaryScreens = new Dictionary<SecondaryUIState, GameObject>
        {
            { SecondaryUIState.Settings, settingsScreen },
            { SecondaryUIState.Controls, controlsScreen }
        };
    }

    private void SceneInitialize()
    {
        GetSensitivitySlider();
        //GetMenuReferences();
    }


    void Start()
    {
        SceneInitialize();
        SetPrimary(PrimaryUIState.Home);
        SetSecondary(SecondaryUIState.None);
    }

    public void SetPrimary(PrimaryUIState newState)
    {
        Debug.Log($"Setting Primary UI Screen to {newState}");

        if (primaryState == newState) return;
        primaryState = newState;

        foreach (var kvp in primaryScreens)
        {
            kvp.Value.SetActive(false);
            Debug.Log($"Setting {kvp.Value} to false", kvp.Value);
        }
            

        if (primaryScreens.TryGetValue(primaryState, out var screen))
        {
            screen.SetActive(true);
            Debug.Log($"Setting {screen} to true", screen);
        }
            

        if (newState == PrimaryUIState.Pause)
        {
            SetSecondary(SecondaryUIState.None);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else if (newState == PrimaryUIState.HUD)
        {
            SetSecondary(SecondaryUIState.None);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void SetSecondary(SecondaryUIState newState)
    {
        Debug.Log($"Setting Secindary UI Screen to {newState}");

        if (secondaryState == newState) return;
        secondaryState = newState;

        foreach (var kvp in secondaryScreens)
            kvp.Value.SetActive(false);

        if (secondaryScreens.TryGetValue(secondaryState, out var screen))
            screen.SetActive(true);
    }

    public void OnGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            UIManager.Instance.GetSensitivitySlider();
            //UIManager.Instance.GetGameReferences();
        }
        SceneManager.sceneLoaded -= OnGameLoaded;
    }
    public void NewGame()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SceneManager.sceneLoaded += OnGameLoaded;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        SetPrimary(PrimaryUIState.HUD);
    }
    public void GoToControls()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.Controls);
    }

    public void GoToSettings()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.Settings);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayOneShot("Button");
        Application.Quit();
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayOneShot("Button");
        AudioManager.Instance.PauseSFX();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetPrimary(PrimaryUIState.Pause);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlayOneShot("Button");
        AudioManager.Instance.ResumeSFX();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetPrimary(PrimaryUIState.HUD);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SceneManager.sceneLoaded += OnMainMenuLoaded;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnMainMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            GetSensitivitySlider();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1;
        }

        SceneManager.sceneLoaded -= OnMainMenuLoaded;
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

    public void GetSensitivitySlider()
    {
        var allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (var slider in allSliders)
        {
            if (slider.name == "SensitivitySlider")
            {
                sensSlider = slider;
                sensSlider.onValueChanged.RemoveAllListeners();
                sensSlider.onValueChanged.AddListener((v) => SensitivityFromSlider());

                LoadSensitivity(); // Load PlayerPrefs into slider.value
                return;
            }
        }

        Debug.LogWarning("Sensitivity slider NOT found in scene!");
    }
}