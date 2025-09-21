using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShipPreset", menuName = "Ships/Ship Preset")]
public class ShipPreset : ScriptableObject
{
    [Header("Info")]
    public string shipName;

    [Header("Movement")]
    public float speed; // multiplier for movement force and boost acceleration
    public float handling; // multiplier for roll force and brake force 
    internal float acceleration;

    [Header("Damage Resistance")]
    public float acidDamageResistance;
    public float impactDamageResistance;

    [Header("Cargo Holding")]
    public float beamStrength;
    public float holdStrength;
}
