using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ShipSelectDisplay : MonoBehaviour
{
    public ShipPreset ship;

    [Header("Info")]

    [SerializeField] private Image displayImage;
    [SerializeField] private TMP_Text displayName;

    [Header("Stats")]

    [SerializeField] private Slider speedBar;
    [SerializeField] private Slider handlingBar;

    [SerializeField] private TMP_Text resistances;

    [SerializeField] private float maxValue = 5f;

    [Header("Purchasing")]

    [SerializeField] private TMP_Text cost;
    [SerializeField] private GameObject locked;

    [SerializeField] private Button purchaseButton;
    [SerializeField] private TMP_Text purchaseButtonText;
    private bool isAvailable => (ShipManager.Instance.OwnedShips.Contains(ship) || GameManager.Instance.Ships[0] == ship || ship.shipCost == 0);
    
    [Header("Audio")]
    private string validSound = "Button";

    private string invalidSound = "Button";

    /*private void OnValidate()
    {
        UpdateShip(ship);

    }*/

    public void UpdateShip(ShipPreset newShip)
    {
        ship = newShip;
        displayImage.sprite = ship.shipIcon;
        displayName.text = ship.shipName;

        speedBar.value = ship.speed / maxValue;
        handlingBar.value = ship.handling / maxValue;

        resistances.text = Resistances();

        cost.text = ship.shipCost.ToString("C0");

        if (isAvailable)
        {
            locked.SetActive(false);
            displayImage.color = Color.white;

            purchaseButton.image.color = Color.white;
            purchaseButtonText.text = "Equip";

            cost.text = "";
        }
        else
        {
            locked.SetActive(true);
            displayImage.color = Color.black;

            purchaseButtonText.text = "Purchase";

            if (CurrencyManager.Instance.CanAfford(ship.shipCost))
            {
                purchaseButton.image.color = Color.white;
                cost.color = Color.white;
            }
            else
            {
                purchaseButton.image.color = Color.red;
                cost.color = Color.red;
            }

        }

        purchaseButton.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });
    }

    private string Resistances()
    {
        string text = "";

        if (ship.acidDamageResistance > 0)
        {
            text += $"{ship.acidDamageResistance * 100:F0}% Acid Resistance\n";
        
        }

        if (ship.impactDamageResistance > 0)
        {
            text += $"{ship.impactDamageResistance * 100:F0}% Impact Resistance\n";

        }

        return text;
    }

    private void OnButtonClicked()
    {
        if (CurrencyManager.Instance.CanAfford(ship.shipCost) || isAvailable)
        {
            AudioManager.Instance.PlayOneShot(validSound);
            ShipManager.Instance.EquipShip(ship);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(invalidSound);
        }

    }
}
