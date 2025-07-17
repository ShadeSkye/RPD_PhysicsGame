using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject HUD;

    public static UIManager instance;

    public Slider sensSlider;
    public float sensFromSlider;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIInitialization.instance.InitializeUI();
        GetSensitivitySlider();
        GetMenuReferences();
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        SceneManager.sceneLoaded += OnGameLoaded;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void OnGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            UIInitialization.instance.InitializeUI();
            VolumeSettings.instance.GetReferences();
            GetSensitivitySlider();
            GetGameReferences();
        }
        SceneManager.sceneLoaded -= OnGameLoaded;
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

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseScreen.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseScreen.SetActive(false);
        controlsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.PlayButtonSFX(UISFX.Button);
        SceneManager.sceneLoaded += OnMainMenuLoaded;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnMainMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            UIInitialization.instance.InitializeUI();
            VolumeSettings.instance.GetReferences();
            GetSensitivitySlider();
            GetMenuReferences();
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

    public void GetMenuReferences()
    {
        homeScreen = GameObject.Find("HomeScreen");

        controlsScreen = GameObject.Find("ControlsScreen");
        controlsScreen.SetActive(false);

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