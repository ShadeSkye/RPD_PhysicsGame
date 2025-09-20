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
    public List<LevelSelectButton> buttons = new();
    public HashSet<LevelData> completedLevels = new HashSet<LevelData>();

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
        foreach (LevelData l in levels)
        {
            LevelSelectButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(l);
            buttons.Add(button);
        }
    }

    public void RefreshButtons()
    {
        foreach (var b in buttons)
        {
            b.UpdateLevelButton();
        }
    }

    internal void OnLevelComplete(LevelData levelData)
    {
        completedLevels.Add(levelData);
        RefreshButtons();
    }
}
