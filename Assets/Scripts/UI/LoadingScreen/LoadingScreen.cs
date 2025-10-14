using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameObject.SetActive(false);
    }

    [SerializeField] private Image img;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button continueButton;

    public void SetLoadingVisuals(LoadingScreenData data)
    {
        img.sprite = data.backgroundImage;
        text.text = data.loadingText;
        title.text = data.loadingTitle;
        continueButton.gameObject.SetActive(data.requireContinueButton);
    }

    public void ShowContinueButton(System.Action onClick)
    {
        continueButton.gameObject.SetActive(true);
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            continueButton.gameObject.SetActive(false);
            onClick?.Invoke();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
