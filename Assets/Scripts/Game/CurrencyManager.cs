using System;
using UnityEngine;
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [HideInInspector] public float CurrentBalance = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentBalance = 0;
    }
    public bool TrySpend(int amount)
    {
        if (!CanAfford(amount)) return false;
        CurrentBalance -= amount;
        CarryingDisplay.Instance.UpdateEarnings();
        return true;
    }

    public bool CanAfford(float value) => CurrentBalance >= value;


    internal void AddEarnings(float value)
    {
        CurrentBalance += value;
        CarryingDisplay.Instance.UpdateEarnings();
    }

    public string CurrencyFormatted(float amount)
    {
        return amount.ToString("C2");
    }

    public string CurrencyFormatted()
    {
        return CurrentBalance.ToString("C2");
    }
}
