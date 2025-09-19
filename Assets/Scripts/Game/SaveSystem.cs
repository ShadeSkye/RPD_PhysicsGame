using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void MarkLevelComplete(int buildIndex)
    {
        PlayerPrefs.SetInt($"Level_{buildIndex}_Complete", 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelComplete(int buildIndex)
    {
        return PlayerPrefs.GetInt($"Level_{buildIndex}_Complete", 0) == 1;
    }
}
