using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarryingDisplay : MonoBehaviour
{
    public static CarryingDisplay Instance { get; private set; }

    [SerializeField] TextMeshProUGUI CarryingName;
    [SerializeField] TextMeshProUGUI CarryingValue;
    [SerializeField] TextMeshProUGUI CarryingDamage;

    public float totalEarnings = 0;
    [SerializeField] TextMeshProUGUI TotalMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ClearCarrying();
        totalEarnings = 0;
        UpdateEarnings();
    }

    public void UpdateCarrying(Cargo cargo)
    {
        CarryingName.text = $"[{cargo.cargoName.ToUpper()}]";
        CarryingValue.text = $"Value: {cargo.CurrentValue.ToString("C2")}";
        CarryingDamage.text = $"Damage: {cargo.damagePercent.ToString("P0")}";
    }

    public void ClearCarrying()
    {
        CarryingName.text = "[No Cargo]";
        CarryingValue.text = "";
        CarryingDamage.text = "";
    }

    public void UpdateEarnings()
    {
        TotalMoney.text = $"Total Earnings: {totalEarnings.ToString("C2")}";
    }
}

