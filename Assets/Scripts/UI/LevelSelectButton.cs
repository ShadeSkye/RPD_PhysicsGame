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
    private LevelData myLevel;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

    private string validSound = "Button";

    private string invalidSound = "Button";

    private bool levelLocked => !LevelSelect.Instance.availableLevels.Contains(myLevel);

    internal void Setup(LevelData level)
    {
        myLevel = level;   
        buttonText.text = level.LevelName;

        button.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });

        UpdateButtonValidity();
    }

    public void UpdateButtonValidity()
    {

        if (buttonImage != null)
            buttonImage.color = levelLocked ? Color.red : Color.white;
    }

    private void OnButtonClicked()
    {
        if (levelLocked)
        {
            AudioManager.Instance.PlayOneShot(invalidSound);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(validSound);
            GameManager.Instance.LoadScene(myLevel.SceneIndex);
        }

    }
}
