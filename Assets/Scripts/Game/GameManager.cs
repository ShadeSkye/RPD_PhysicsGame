using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;
using static UIManager;

public enum SceneIndex
{
    MainMenu,
    Level1,
    Level2,
    Level3
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Load Transition")]
    [SerializeField] private Image fadeImage;
    private float fadeDuration = 0.5f;

    public bool GamePaused = false;

    [Header("Ships")]
    public List<ShipPreset> Ships = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Pause()
    {
        AudioManager.Instance.PauseSFX();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        GamePaused = true;
    }

    public void Play()
    {
        AudioManager.Instance.ResumeSFX();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        GamePaused = false;
    }

    public void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
        {
            LoadScene((SceneIndex)next);
        }
        else
        {
            LoadScene((int)SceneIndex.MainMenu);
        }
    }

    public void LoadScene(SceneIndex scene)
    {
        AudioManager.Instance.StopAllSFX();

        StartCoroutine(FadeAndLoad(scene, true));
    }

    public void LoadScene(SceneIndex scene, float duration)
    {

        StartCoroutine(FadeAndLoad(scene, true));
    }

    private IEnumerator FadeAndLoad(SceneIndex scene, bool doFade)
    {
        if(doFade) yield return StartCoroutine(Fade(0f, 1f, scene));

        AsyncOperation op = SceneManager.LoadSceneAsync((int)scene);
        while (!op.isDone)
        {
            yield return null;
        }

        OnSceneLoad(scene);

        if (doFade) yield return StartCoroutine(Fade(1f, 0f, scene));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, SceneIndex scene)
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private void OnSceneLoad(SceneIndex scene)
    {
        AudioManager.Instance.PlayAllSFX();

        switch (scene)
        {
            case SceneIndex.MainMenu:
                SaveManager.Instance.UpdateMainMenu();
                UIManager.Instance.SetPrimary(PrimaryUIState.Home);
                Pause();
                break;
            default:
                UIManager.Instance.SetPrimary(PrimaryUIState.HUD);
                Play();
                break;

        }
    }

    public void NewGame()
    {
        SaveManager.Instance.ResetProgress();
        LoadScene(SceneIndex.Level1);
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadProgress();
        LoadScene(SaveManager.Instance.LoadLastCompletedLevel());
    }
}
