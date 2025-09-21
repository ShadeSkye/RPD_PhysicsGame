using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ShipStats : MonoBehaviour
{
    public static ShipStats Instance;
    [SerializeField] private ShipPreset preset;
    private InputManager input;

    public float Speed => preset.speed;
    public float Handling => preset.handling;

    public float AcidDamageResistance => preset.acidDamageResistance;
    public float ImpactDamageResistance => preset.impactDamageResistance;

    public float BeamStrength => preset.beamStrength;
    public float HoldStrength => preset.holdStrength;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void LateUpdate()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        input = InputManager.Instance;

        if (input != null)
        {
            input.movementForce = input.defaultMovementForce * Speed;
            input.boostRate = input.defaultBoostRate * Speed;

            input.rollForce = input.defaultRollForce * Handling;
            input.brakeForce = input.defaultBrakeForce * Handling / 2;

        }

    }
}
