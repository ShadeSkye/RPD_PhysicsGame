using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum UIButtonAction
{
    NewGame,
    Resume,
    LoadGame,
    OpenSettings,
    OpenControls,
    QuitGame,
    MainMenu,
    RestartLevel,
    ResumeResetPosition
}

public class UIButtonLink : MonoBehaviour
{
    public UIButtonAction action;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (UIManager.Instance  == null)
        {
            Debug.Log("UIManager not found!");
            return;
        }

        switch (action)
        {
            case UIButtonAction.NewGame:
                button.onClick.AddListener(UIManager.Instance.NewGame);
                break;
            case UIButtonAction.Resume:
                button.onClick.AddListener(UIManager.Instance.ResumeGame);
                break;
            case UIButtonAction.ResumeResetPosition:
                button.onClick.AddListener(UIManager.Instance.ResumeResetPosition);
                break;
            case UIButtonAction.LoadGame:
                button.onClick.AddListener(UIManager.Instance.LoadGame);
                break;
            case UIButtonAction.OpenSettings:
                button.onClick.AddListener(UIManager.Instance.OpenSettings);
                break;
            case UIButtonAction.OpenControls:
                button.onClick.AddListener(UIManager.Instance.OpenControls);
                break;
            case UIButtonAction.QuitGame:
                button.onClick.AddListener(UIManager.Instance.QuitGame);
                break;
            case UIButtonAction.MainMenu:
                button.onClick.AddListener(UIManager.Instance.MainMenu);
                break;
            case UIButtonAction.RestartLevel:
                button.onClick.AddListener(UIManager.Instance.RestartLevel);
                break;
            default:
                Debug.LogWarning("UIButtonLink: Unhandled action on " + gameObject.name);
                break;
        }
    }
}
