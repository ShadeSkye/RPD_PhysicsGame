using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectButton : MonoBehaviour
{
    private ShipPreset myShip;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

    private string validSound = "Button";

    private string invalidSound = "Button";


    internal void Setup(ShipPreset ship)
    {
        myShip = ship;
        buttonText.text = $"{ship.shipName}\n{CurrencyManager.Instance.CurrencyFormatted(ship.shipCost)}";

        button.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });

        UpdateButtonValidity();
    }

    public void UpdateButtonValidity()
    {

        if (buttonImage != null)
            buttonImage.color = !CurrencyManager.Instance.CanAfford(myShip.shipCost) ? Color.red : Color.white;
    }

    private void OnButtonClicked()
    {
        if (!CurrencyManager.Instance.CanAfford(myShip.shipCost))
        {
            AudioManager.Instance.PlayOneShot(invalidSound);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(validSound);
            ShipSelect.Instance.SetSelectedShip(myShip);
        }

    }
}
