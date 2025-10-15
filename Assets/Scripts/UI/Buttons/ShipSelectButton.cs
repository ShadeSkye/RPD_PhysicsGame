using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShipSelectButton : DefaultButton
{
    private ShipPreset myShip;
    [SerializeField] private TMP_Text buttonText;
    //[SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

   /* private string validSound = "Button";

    private string invalidSound = "Button";

    private bool isAvailable => (ShipManager.Instance.OwnedShips.Contains(myShip) || GameManager.Instance.Ships[0] == myShip || myShip.shipCost == 0);
*/
    internal void Setup(ShipPreset ship)
    {
        myShip = ship;

        buttonText.text = $"{myShip.shipName}";

        /*button.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });

        UpdateButtonValidity();*/
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShipSelect.Instance.SetDisplayedShip(myShip);
    }

    /*public void UpdateButtonValidity()
    {
        if (isAvailable)
        {
            buttonText.text = $"{myShip.shipName}";
            buttonImage.color = Color.white;
        }
        else
        {
            buttonText.text = $"{myShip.shipName}\n{CurrencyManager.Instance.CurrencyFormatted(myShip.shipCost)}";

            if (buttonImage != null)
                buttonImage.color = !CurrencyManager.Instance.CanAfford(myShip.shipCost) ? Color.red : Color.white;
        }
        
    }*/

    /*private void OnButtonClicked()
    {
        if (CurrencyManager.Instance.CanAfford(myShip.shipCost) || isAvailable)
        {
            AudioManager.Instance.PlayOneShot(validSound);
            ShipManager.Instance.EquipShip(myShip);
        }
        else
        {
            AudioManager.Instance.PlayOneShot(invalidSound);
        }

    }*/
}
