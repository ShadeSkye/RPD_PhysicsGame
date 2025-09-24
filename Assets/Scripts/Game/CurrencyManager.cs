using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
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
    public bool TrySpend(float value)
    {
        if (!CanAfford(value)) return false;
        CurrentBalance -= value;
        CarryingDisplay.Instance.UpdateEarnings();
        //SaveManager.Instance.SaveCredits(CurrentBalance);
        return true;
    }

    public bool CanAfford(float value) => CurrentBalance >= value;

    public void ClearCredits()
    {
        CurrentBalance = 0;
        CarryingDisplay.Instance.UpdateEarnings();

        ShipSelect.Instance.RefreshButtons();
    }

    public void LoadCredits(float value)
    {
        CurrentBalance = value;
        CarryingDisplay.Instance.UpdateEarnings();

        ShipSelect.Instance.RefreshButtons();
    }

    internal void AddEarnings(float value)
    {
        CurrentBalance += value;
        CarryingDisplay.Instance.UpdateEarnings();
        //SaveManager.Instance.SaveCredits(CurrentBalance);

        ShipSelect.Instance.RefreshButtons();
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
