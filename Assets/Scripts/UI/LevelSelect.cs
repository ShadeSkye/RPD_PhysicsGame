using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect Instance;

    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private LevelSelectButton buttonPrefab;

    [SerializeField] private Button completeLevelButton;

    public List<LevelData> levels = new();
    [HideInInspector] public List<LevelSelectButton> buttons = new();
    [HideInInspector] public HashSet<LevelData> completedLevels = new HashSet<LevelData>();

    private int lastCompletedIndex;
    public bool currentLevelComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    void Start()
    {
        lastCompletedIndex = (int)SaveManager.Instance.LoadLastCompletedLevel();

        ResetButtons();

        foreach (LevelData l in levels)
        {
            LevelSelectButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(l);

            //Debug.Log($"complete: {(int)l.SceneIndex <= lastCompletedIndex} (this scene {(int)l.SceneIndex} comes before loaded scene {lastCompletedIndex}?");

            buttons.Add(button);

            if(SaveManager.Instance.IsGameInProgress() && (int)l.SceneIndex <= lastCompletedIndex)
            {
                completedLevels.Add(l);
                Debug.Log($"Level select buttons: {l} marked complete");
            }
        }

        RefreshButtons();
    }

    public void ResetButtons()
    {
        currentLevelComplete = false;
        RefreshButtons();
    }

    public void RefreshButtons()
    {

        completeLevelButton.gameObject.SetActive(currentLevelComplete);

        foreach (var b in buttons)
        {
            b.UpdateButtonValidity();
        }


    }

    internal void OnLevelComplete(LevelData levelData)
    {
        currentLevelComplete = true;

        completedLevels.Add(levelData);

        RefreshButtons();
    }
}
