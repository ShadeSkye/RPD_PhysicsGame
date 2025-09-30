using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject ResumeButtonPrefab;
    public GameObject HangarResumeButtonPrefab;

    [Header("Sensitivity")]
    public Slider sensSlider;
    public float sensFromSlider;

    private PrimaryUIState primaryState;
    private SecondaryUIState secondaryState;

    [Header("Primary Screens")]
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject hangarScreen;
    [SerializeField] private GameObject objectiveSelection;

    [Header("Secondary Screens")]
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject shipSelect;
    [SerializeField] private GameObject levelSelect;

    private Dictionary<PrimaryUIState, GameObject> primaryScreens;
    private Dictionary<SecondaryUIState, GameObject> secondaryScreens;

    //[SerializeField] private Image pullZoneIndicator;
    public enum PrimaryUIState
    {
        None,
        HUD,
        Home,
        PauseMenu,
        HangarMenu,
        ObjectiveSelect,
    }

    public enum SecondaryUIState
    {
        None,
        Settings,
        Controls,
        ShipSelect,
        LevelSelect,
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
            { PrimaryUIState.HUD, HUD },
            { PrimaryUIState.Home, homeScreen },
            { PrimaryUIState.PauseMenu, pauseScreen },
            { PrimaryUIState.HangarMenu, hangarScreen },
            { PrimaryUIState.ObjectiveSelect, objectiveSelection },
        };

        secondaryScreens = new Dictionary<SecondaryUIState, GameObject>
        {
            { SecondaryUIState.Settings, settingsScreen },
            { SecondaryUIState.Controls, controlsScreen },
            { SecondaryUIState.ShipSelect, shipSelect },
            { SecondaryUIState.LevelSelect, levelSelect },
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
            SaveManager.Instance.UpdateMainMenu();
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

    public void ShipSelect()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.ShipSelect);
    }

    public void LevelSelect()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.LevelSelect);
    }

    public void ObjectiveSelect()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetPrimary(PrimaryUIState.ObjectiveSelect);
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

    public void OpenShipSelect()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.ShipSelect);
    }

    public void OpenLevelSelect()
    {
        AudioManager.Instance.PlayOneShot("Button");
        SetSecondary(SecondaryUIState.LevelSelect);
    }

    public void OpenHangar()
    {
        GameManager.Instance.Pause();

        AudioManager.Instance.PlayOneShot("Button");

        SetPrimary(PrimaryUIState.HangarMenu);
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
        
        SetPrimary(menuType);
    }

    public void ResumeGame()
    {
        Debug.Log($"UIMANAGER: Resume game");
        GameManager.Instance.Play();

        AudioManager.Instance.PlayOneShot("Button");

        SetPrimary(PrimaryUIState.HUD);
    }

    public void ResumeResetPosition()
    {
        PlayerManager.Instance.ResetLocation(LevelManager.Instance.SpaceStation.transform);

        ResumeGame();
    }

    public void MainMenu()
    {
        Debug.Log("Going to main menu");
        GameManager.Instance.LoadScene(SceneIndex.MainMenu);

        AudioManager.Instance.PlayOneShot("Button");
    }

    public void NewGame()
    {
        GameManager.Instance.NewGame();

        AudioManager.Instance.PlayOneShot("Button");

    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();

        AudioManager.Instance.PlayOneShot("Button");

    }

    public void RestartLevel()
    {

        GameManager.Instance.RestartLevel();

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

    /*public void CargoInRange(bool isInRange)
    {
        pullZoneIndicator.color = isInRange ? Color.yellow : Color.white;

    }*/

    public string StringTime(float time)
    {
        int totalSeconds = Mathf.CeilToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string txt = "";

        if (minutes > 0)
        {
            txt += $"{minutes} min ";
        }

        txt += $"{seconds} sec";

        return txt;
    }
}