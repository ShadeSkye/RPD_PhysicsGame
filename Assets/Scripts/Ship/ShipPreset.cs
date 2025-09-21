using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShipPreset", menuName = "Ships/Ship Preset")]
public class ShipPreset : ScriptableObject
{
    [Header("Info")]
    public string shipName;

    [Header("Movement")]
    public float speed = 1; // multiplier for movement force and boost acceleration
    public float handling = 1; // multiplier for roll force and brake force 

    [Header("Damage Resistance")]
    public float acidDamageResistance = 0; // percent resisting acid damage
    public float impactDamageResistance = 0; // percent resisting impact damage

    /*[Header("Cargo Holding")]
    public float beamStrength;
    public float holdStrength;*/
}
