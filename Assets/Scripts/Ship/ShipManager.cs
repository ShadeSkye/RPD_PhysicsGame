using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    public List<ShipPreset> OwnedShips = new();

    public ShipPreset CurrentShip;

    public float Speed => CurrentShip.speed;
    public float Handling => CurrentShip.handling;

    public float AcidDamageResistance => CurrentShip.acidDamageResistance;
    public float ImpactDamageResistance => CurrentShip.impactDamageResistance;

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
        CurrentShip = GameManager.Instance.Ships[0];
        OwnedShips.Clear();

        ShipSelect.Instance.RefreshButtons();
    }


    public void EquipShip(ShipPreset ship)
    {
        if (!OwnedShips.Contains(ship))
        {
            if (!PurchaseShip(ship)) return;
        }

        CurrentShip = ship;

        //SaveManager.Instance.SaveShips(OwnedShips, CurrentShip);

        UIManager.Instance.ResumeResetPosition();

    }

    public bool PurchaseShip(ShipPreset ship)
    {  
        if (CurrencyManager.Instance.TrySpend(ship.shipCost))
        {
            OwnedShips.Add(ship);
            //SaveManager.Instance.SaveShips(OwnedShips, CurrentShip);
            ShipSelect.Instance.RefreshButtons();

            return true;
        }

        return false;
        
    }

    public void LoadShips(List<ShipPreset> ships, ShipPreset equipped)
    {
        OwnedShips = ships;
        CurrentShip = equipped;

        ShipSelect.Instance.RefreshButtons();
    }

}
