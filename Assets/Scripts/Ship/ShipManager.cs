using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    public List<ShipPreset> OwnedShips = new();

    [SerializeField] private ShipPreset preset;

    public float Speed => preset.speed;
    public float Handling => preset.handling;

    public float AcidDamageResistance => preset.acidDamageResistance;
    public float ImpactDamageResistance => preset.impactDamageResistance;

    /*public float BeamStrength => preset.beamStrength;
    public float HoldStrength => preset.holdStrength;*/

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ClearShips()
    {
        preset = GameManager.Instance.Ships[0];
        OwnedShips.Clear();
    }


    public void EquipShip(ShipPreset ship)
    {
        if (!OwnedShips.Contains(ship))
        {
            if (!PurchaseShip(ship)) return;
        }

        preset = ship; 
        UIManager.Instance.ResumeGame();

    }

    public bool PurchaseShip(ShipPreset ship)
    {  
        if (CurrencyManager.Instance.TrySpend(ship.shipCost))
        {
            OwnedShips.Add(ship);
            SaveManager.Instance.SaveOwnedShips(OwnedShips);
            ShipSelect.Instance.RefreshButtons();

            return true;
        }

        return false;
        
    }

}
