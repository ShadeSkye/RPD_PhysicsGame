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

    public List<LevelData> levels = new();
    [HideInInspector] public List<LevelSelectButton> buttons = new();
    [HideInInspector] public HashSet<LevelData> completedLevels = new HashSet<LevelData>();

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
        int lastCompletedIndex = (int)SaveManager.Instance.LoadLastCompletedLevel();

        foreach (LevelData l in levels)
        {
            LevelSelectButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(l);
            buttons.Add(button);

            if(SaveManager.Instance.IsGameInProgress() && (int)l.SceneIndex <= lastCompletedIndex)
            {
                completedLevels.Add(l);
            }
        }

        RefreshButtons();
    }

    public void RefreshButtons()
    {
        foreach (var b in buttons)
        {
            b.UpdateButtonValidity();
        }
    }

    internal void OnLevelComplete(LevelData levelData)
    {
        completedLevels.Add(levelData);

        RefreshButtons();
    }
}
