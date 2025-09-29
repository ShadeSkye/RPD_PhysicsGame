using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarSensor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered hangar");

            UIManager.Instance.PauseGame(UIManager.PrimaryUIState.ShipSelect);
        }
    }
}
