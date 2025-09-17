using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthDisplay;

    void Update()
    {
        float percent = PlayerManager.Instance.damagePercent * 100;
        healthDisplay.text = $"SHIP DAMAGE: {percent:F0}%";
    }
}