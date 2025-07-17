using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum UIButtonAction
{
    Resume,
    GoToSettings,
    GoToControls,
    QuitGame,
    GoToMainMenu
}

public class UIButtonLink : MonoBehaviour
{
    public UIButtonAction action;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (UIManager.instance == null)
        {
            Debug.LogError("UIManager not found!");
            return;
        }

        switch (action)
        {
            case UIButtonAction.Resume:
                button.onClick.AddListener(UIManager.instance.ResumeGame);
                break;
            case UIButtonAction.GoToSettings:
                button.onClick.AddListener(UIManager.instance.GoToSettings);
                break;
            case UIButtonAction.GoToControls:
                button.onClick.AddListener(UIManager.instance.GoToControls);
                break;
            case UIButtonAction.QuitGame:
                button.onClick.AddListener(UIManager.instance.QuitGame);
                break;
            case UIButtonAction.GoToMainMenu:
                button.onClick.AddListener(UIManager.instance.GoToMainMenu);
                break;
            default:
                Debug.LogWarning("UIButtonLink: Unhandled action on " + gameObject.name);
                break;
        }
    }
}
