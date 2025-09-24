using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string LastCompletedKey = "LastCompletedLevel";
    private string CreditsKey = "Credits";
    private string EquippedShipKey = "EquippedShip";
    private string OwnedShipsKey = "OwnedShips";

    [SerializeField] private GameObject loadGameButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void SaveLastCompletedLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(LastCompletedKey, levelIndex);

        PlayerPrefs.Save();
    }

    public SceneIndex LoadLastCompletedLevel()
    {
        return (SceneIndex)PlayerPrefs.GetInt(LastCompletedKey, -1);
    }

    public void SaveCredits(float amount)
    {
        PlayerPrefs.SetFloat(CreditsKey, amount);
        PlayerPrefs.Save();
    }

    public float LoadCredits()
    {
        return PlayerPrefs.GetFloat(CreditsKey, 0f);
    }

    public void SaveShips(List<ShipPreset> ownedShips, ShipPreset equippedShip)
    {
        List<int> ownedShipIDs = new List<int>();

        foreach (ShipPreset s in ownedShips)
        {
            int id = GameManager.Instance.Ships.IndexOf(s);
            if (id != -1)
            {
                ownedShipIDs.Add(id);
            }
            else
            {
                Debug.LogWarning("ShipPreset not found in GameManager.Ships!");
            }
        }

        string data = string.Join(",", ownedShipIDs);

        PlayerPrefs.SetString(OwnedShipsKey, data);

        int equippedID = GameManager.Instance.Ships.IndexOf(equippedShip);
        if (equippedID != -1)
        {
            PlayerPrefs.SetInt(EquippedShipKey, equippedID);
        }
        else
        {
            Debug.LogWarning("Equipped ship not found in GameManager.Ships!");
        }

        PlayerPrefs.Save();
    }

    public List<ShipPreset> LoadOwnedShips()
    {
        string data = PlayerPrefs.GetString(OwnedShipsKey, "0");

        List<ShipPreset> ownedShips = new List<ShipPreset>();

        foreach (var s in data.Split(','))
        {
            if (int.TryParse(s, out int id))
            {
                if (id >= 0 && id < GameManager.Instance.Ships.Count)
                    ownedShips.Add(GameManager.Instance.Ships[id]);
                else
                    Debug.LogWarning($"Saved ship index {id} is invalid!");
            }
                
        }

        return ownedShips;
    }

    public ShipPreset LoadEquippedShip()
    {
        int equippedID = PlayerPrefs.GetInt(EquippedShipKey, -1);

        if (equippedID >= 0 && equippedID < GameManager.Instance.Ships.Count)
        {

            return GameManager.Instance.Ships[equippedID];
        }

        Debug.LogWarning($"Equipped ship index {equippedID} is invalid!");
        return null;
    }

    public bool IsGameInProgress()
    {
        return PlayerPrefs.HasKey(LastCompletedKey);
    }

    public void UpdateMainMenu()
    {
        loadGameButton.SetActive(IsGameInProgress());
    }

    public void ResetProgress()
    {
        CurrencyManager.Instance.ClearCredits();
        ShipManager.Instance.ClearShips();

        PlayerPrefs.DeleteKey(LastCompletedKey);
        PlayerPrefs.DeleteKey(CreditsKey);
        PlayerPrefs.DeleteKey(EquippedShipKey);
        PlayerPrefs.DeleteKey(OwnedShipsKey);
        PlayerPrefs.Save();
    }

    public void SaveProgress()
    {
        SaveCredits(CurrencyManager.Instance.CurrentBalance);
        SaveShips(ShipManager.Instance.OwnedShips, ShipManager.Instance.CurrentShip);

    }

    internal void LoadProgress()
    {
        CurrencyManager.Instance.LoadCredits(LoadCredits());
        ShipManager.Instance.LoadShips(LoadOwnedShips(), LoadEquippedShip());   
    }
}
