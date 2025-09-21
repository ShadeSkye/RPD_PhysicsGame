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

    [SerializeField] TextMeshProUGUI TotalMoney;

    public Cargo CurrentCargo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    private void Start()
    {

        ClearCarrying();
        UpdateEarnings();
    }

    public void SetCarrying(Cargo cargo)
    {
        CurrentCargo = cargo;
        UpdateCarrying();
    }

    public void UpdateCarrying()
    {
        if (CurrentCargo != null)
        {
            CarryingName.text = $"[{CurrentCargo.objectName.ToUpper()}]";
            CarryingValue.text = $"Value: {CurrentCargo.CurrentValue.ToString("C2")}";
            CarryingDamage.text = $"Damage: {CurrentCargo.DamagePercent.ToString("P0")}";
        }
        else
        {
            ClearCarrying();
        }
    }

    public void ClearCarrying()
    {
        CurrentCargo = null;
        CarryingName.text = "[No Cargo]";
        CarryingValue.text = "";
        CarryingDamage.text = "";
    }

    public void UpdateEarnings()
    {
        TotalMoney.text = $"Total Earnings: {CurrencyManager.Instance.CurrencyFormatted()}";
    }
}

