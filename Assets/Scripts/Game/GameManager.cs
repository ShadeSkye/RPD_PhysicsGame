using Cinemachine;
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
    MainMenu = 0,
    Game = 1,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Load Transition")]
    [SerializeField] private Image fadeImage;
    private float fadeDuration = 0.5f;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void LoadScene(SceneIndex scene)
    {
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
        switch (scene)
        {
            case SceneIndex.MainMenu:
                UIManager.Instance.SetPrimary(PrimaryUIState.Home);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1;
                break;
            case SceneIndex.Game:
                UIManager.Instance.SetPrimary(PrimaryUIState.HUD);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                break;

        }
    }

}
