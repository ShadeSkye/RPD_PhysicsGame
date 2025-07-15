using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfo : MonoBehaviour
{
    [Header("Plug in text")]
    [SerializeField] TextMeshProUGUI objectName;
    [SerializeField] TextMeshProUGUI distance;
    [SerializeField] GameObject objectInfoUI;

    /// <summary>
    /// Simple if statement changing if the UI should be active based on what showUI returns (True to set active, False to deactivate)
    /// </summary>
    /// <param name="showUI"></param>
    private void DisplayObjectInfoUI(bool showUI)
    {
        if (showUI)
        {
            objectInfoUI.SetActive(true);
        }
        else if (!showUI)
        {
            objectInfoUI.SetActive(false);
        }
    }

    /// <summary>
    /// DisplayObjectName sets the name to the string that is passed through
    /// </summary>
    /// <param name="name"></param>
    private void DisplayObjectName(string name)
    {
        objectName.text = name;
    }

    /// <summary>
    /// DisplayObjectDistance sets the distance to the float that is passed through rounded to the nearest int
    /// </summary>
    /// <param name="dist"></param>
    private void DisplayObjectDistance(float dist)
    {
        dist = Mathf.RoundToInt(dist);
        distance.text = $"Distance: {dist} M";
    }
}
