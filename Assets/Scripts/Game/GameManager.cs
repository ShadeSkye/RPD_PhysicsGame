using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public LevelData[] Levels;

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

        for (int i = 0; i < Levels.Length; i++)
        {
            if (Levels[i] != null)
            {
                Levels[i].SceneIndex = (SceneIndex)i;
            }
        }
    }

    public SceneIndex GetCurrentScene()
    {
        return (SceneIndex)SceneManager.GetActiveScene().buildIndex;
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

    public void RestartLevel()
    {
        SaveManager.Instance.LoadProgress();
        LoadScene(GameManager.Instance.GetCurrentScene());
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

        SaveManager.Instance.SaveLastCompletedLevel(current);
    }

    public void LoadScene(SceneIndex scene)
    {
        AudioManager.Instance.StopAllSFX();

        StartCoroutine(FadeAndLoad(scene));

    }

    private IEnumerator FadeAndLoad(SceneIndex scene)
    {
        yield return StartCoroutine(Fade(0f, 1f, scene)); 

        AsyncOperation op = SceneManager.LoadSceneAsync((int)scene);
        while (!op.isDone)
        {
            yield return null;
        }

        OnSceneLoad(scene);

        // get loading data
        LoadingScreenData loadingScreen = null;
        if (LevelManager.Instance != null && LevelManager.Instance.LevelData != null)
        {
            loadingScreen = LevelManager.Instance.LevelData.loadingScreen;
        }

        if (loadingScreen != null)
        {
            LoadingScreen.Instance.SetLoadingVisuals(loadingScreen);
            LoadingScreen.Instance.Show();

            Pause();

            // timer
            float t = 0f;
            while (t < loadingScreen.holdTime)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            if (loadingScreen.requireContinueButton)
            {
                bool clicked = false;
                LoadingScreen.Instance.ShowContinueButton(() => clicked = true);

                while (!clicked)
                    yield return null;
            }

            LoadingScreen.Instance.Hide();

            Play();

            yield return StartCoroutine(Fade(1f, 0f, scene));
        }
        else
        {
            yield return StartCoroutine(Fade(1f, 0f, scene));
        }


    }

    private IEnumerator Fade(float startAlpha, float endAlpha, SceneIndex scene)
    {
        float t = 0f;
        Color color = fadeImage.color;

        // timer
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
                //LevelManager.Instance.AssignLevelData(Levels[(int)scene]);
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

        int nextLevelIndex = (int)SaveManager.Instance.LoadLastCompletedLevel() + 1;
        nextLevelIndex = Mathf.Min(nextLevelIndex, (int)LevelSelect.Instance.levels.Max(l => l.SceneIndex));

        Debug.Log($"Loading scene {(SceneIndex)nextLevelIndex}");

        LoadScene((SceneIndex)nextLevelIndex);

    }
}
