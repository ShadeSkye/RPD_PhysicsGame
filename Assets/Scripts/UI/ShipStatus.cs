using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipStatus : MonoBehaviour
{
    [Header("Plug in text")]
    [SerializeField] TextMeshProUGUI Money;
    [SerializeField] TextMeshProUGUI Weight;

    ShipStatus instance;

    private void Awake()
    {
        instance = this;
    }

    private void DisplayCredits(int money)
    {
        Money.text = $"Credits: ${money}";
    }

    private void DisplayWeight(float mass)
    {
        Weight.text = $"Weight: {mass}kg";
    }
}
