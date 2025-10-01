using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static Cinemachine.DocumentationSortingAttribute;
public class ShipSelect : MonoBehaviour
{
    public static ShipSelect Instance;
    [HideInInspector] public List<ShipSelectButton> buttons = new();

    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private ShipSelectButton buttonPrefab;

    [SerializeField] private ShipSelectDisplay shipDisplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    void Start()
    {

        foreach (ShipPreset s in GameManager.Instance.Ships)
        {
            ShipSelectButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(s);
            buttons.Add(button);

        }

        //RefreshButtons();
    }

    /*public void RefreshButtons()
    {
        foreach (var b in buttons)
        {
            b.UpdateButtonValidity();
        }
    }*/

    public void SetDisplayedShip(ShipPreset selected)
    {
        shipDisplay.UpdateShip(selected);
    }

}
