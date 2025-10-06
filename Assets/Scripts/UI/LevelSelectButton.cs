using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelSelectButton : MonoBehaviour
{
    public LevelData Level;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

    private string validSound = "Button";

    private string invalidSound = "Button";

    private bool levelLocked => !LevelSelect.Instance.completedLevels.Contains(Level) && !currentLevel;
    private bool currentLevel => (int)LevelManager.Instance.LevelData.SceneIndex == (int)Level.SceneIndex;
    private bool nextLevel => (int)Level.SceneIndex == (int)LevelManager.Instance.LevelData.SceneIndex+1 && LevelSelect.Instance.currentLevelComplete;

    internal void Setup(LevelData level)
    {
        Level = level;   
        buttonText.text = level.LevelName;

        button.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });

        UpdateButtonValidity();
    }

    public void UpdateButtonValidity()
    {
        if (buttonImage != null && (LevelManager.Instance != null))
        {
            if (nextLevel) buttonImage.color = Color.yellow;
            else if (levelLocked) buttonImage.color = Color.red;
            else buttonImage.color = Color.white;
        }
    }

    private void OnButtonClicked()
    {
        if (levelLocked && !nextLevel)
        {
            AudioManager.Instance.PlayOneShot(invalidSound);
        }
        else if (nextLevel)
        {
            AudioManager.Instance.PlayOneShot(validSound);
            UIManager.Instance.LevelComplete();
        }
        else
        {
            AudioManager.Instance.PlayOneShot(validSound);
            GameManager.Instance.LoadScene(Level.SceneIndex);
        }

    }
}
