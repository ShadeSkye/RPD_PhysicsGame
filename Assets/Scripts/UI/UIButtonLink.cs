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
            case UIButtonAction.Resume:
                button.onClick.AddListener(UIManager.Instance.ResumeGame);
                break;
            case UIButtonAction.GoToSettings:
                button.onClick.AddListener(UIManager.Instance.GoToSettings);
                break;
            case UIButtonAction.GoToControls:
                button.onClick.AddListener(UIManager.Instance.GoToControls);
                break;
            case UIButtonAction.QuitGame:
                button.onClick.AddListener(UIManager.Instance.QuitGame);
                break;
            case UIButtonAction.GoToMainMenu:
                button.onClick.AddListener(UIManager.Instance.GoToMainMenu);
                break;
            default:
                Debug.LogWarning("UIButtonLink: Unhandled action on " + gameObject.name);
                break;
        }
    }
}
