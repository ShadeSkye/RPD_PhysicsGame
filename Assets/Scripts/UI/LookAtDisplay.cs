using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LookAtDisplay : MonoBehaviour
{
    public static LookAtDisplay Instance { get; private set; }

    [SerializeField] TextMeshProUGUI ObjectName;
    [SerializeField] TextMeshProUGUI Distance;
    //[SerializeField] GameObject ObjectInfoUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
/*
    /// <summary>
    /// Simple if statement changing if the UI should be active based on what showUI returns (True to set active, False to deactivate)
    /// </summary>
    /// <param name="showUI"></param>
    private void DisplayObjectInfoUI(bool showUI)
    {
        if (showUI)
        {
            ObjectInfoUI.SetActive(true);
        }
        else if (!showUI)
        {
            ObjectInfoUI.SetActive(false);
        }
    }*/

    /// <summary>
    /// DisplayObjectName sets the name to the string that is passed through
    /// </summary>
    /// <param name="name"></param>
    private void UpdateName(string name)
    {
        if (string.IsNullOrEmpty(name)) name = "Unknown Object";
        ObjectName.text = name;
    }

    /// <summary>
    /// DisplayObjectDistance sets the distance to the float that is passed through rounded to the nearest int
    /// </summary>
    /// <param name="dist"></param>
     private void UpdateDistance(float dist)
     {
        string formatted = dist.ToString("F0");
        Distance.text = $"Distance: {formatted}m";
     }

     public void UpdateLookAtObject(string name, float dist)
     {
        UpdateName(name);
        UpdateDistance(dist);
     }

    public void ClearDisplay()
    {
        ObjectName.text = "";
        Distance.text = "";
    }
}
