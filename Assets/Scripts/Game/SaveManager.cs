using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string LastCompletedKey = "LastCompletedLevel";

    [SerializeField] private GameObject loadGameButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void SaveLastCompletedLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(LastCompletedKey, levelIndex);
        PlayerPrefs.Save();
    }

    public SceneIndex LoadLastCompletedLevel()
    {
        return (SceneIndex)PlayerPrefs.GetInt(LastCompletedKey, -1);
    }

    public bool IsGameInProgress()
    {
        return PlayerPrefs.HasKey(LastCompletedKey);
    }

    public void UpdateMainMenu()
    {
        loadGameButton.SetActive(IsGameInProgress());
    }
}
