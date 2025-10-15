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

    [SerializeField] private GameObject shipModel;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        meshFilter = shipModel?.GetComponent<MeshFilter>();
        meshRenderer = shipModel?.GetComponent<MeshRenderer>();
    }


    private void Start()
    {
        LoadShips(OwnedShips, OwnedShips[0]);
    }

    public void ClearShips()
    {
        EquipShip(GameManager.Instance.Ships[0]);
        OwnedShips.Clear();
        OwnedShips.Add(CurrentShip);

        //ShipSelect.Instance.RefreshButtons();
    }


    public void EquipShip(ShipPreset ship)
    {
        CurrentShip = ship;
        meshFilter.mesh = ship.mesh;
        meshRenderer.material = ship.material;

        if(LevelManager.Instance !=null)
            UIManager.Instance.ResumeResetPosition();

        //SaveManager.Instance.SaveShips(OwnedShips, CurrentShip);

    }

    public void PurchaseShip(ShipPreset ship)
    {  
        if (CurrencyManager.Instance.TrySpend(ship.shipCost))
        {
            OwnedShips.Add(ship);
        }
        
    }

    public void LoadShips(List<ShipPreset> ships, ShipPreset equipped)
    {
        OwnedShips = ships;
        CurrentShip = equipped;

        //ShipSelect.Instance.RefreshButtons();
    }

}
