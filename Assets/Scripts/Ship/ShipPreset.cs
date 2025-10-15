using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ShipPreset", menuName = "Ships/Ship Preset")]
public class ShipPreset : ScriptableObject
{
    public Sprite shipIcon;

    [Header("Info")]
    public string shipName;
    public float shipCost;

    [Header("Movement")]
    public float speed = 1; // multiplier for movement force and boost acceleration
    public float handling = 1; // multiplier for roll force and brake force 

    [Header("Damage Resistance")]
    public float acidDamageResistance = 0; // percent resisting acid damage
    public float impactDamageResistance = 0; // percent resisting impact damage

    [Header("Visuals")]
    public Mesh mesh;
    public Material material;

    /*[Header("Cargo Holding")]
    public float beamStrength;
    public float holdStrength;*/
}
