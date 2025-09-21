using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthDisplay;
    [SerializeField] TextMeshProUGUI speedDisplay;

    void Update()
    {
        if(PlayerManager.Instance != null)
        {
            float percent = PlayerManager.Instance.damagePercent * 100;
            healthDisplay.text = $"SHIP DAMAGE: {percent:F0}%";

            float speed = PlayerManager.Instance.GetComponent<Rigidbody>().velocity.magnitude;
            speedDisplay.text = $"SPEED: {speed:F1}";
        }
    }
}