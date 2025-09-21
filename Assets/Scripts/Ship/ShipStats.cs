using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ShipStats : MonoBehaviour
{
    public static ShipStats Instance;
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

}
