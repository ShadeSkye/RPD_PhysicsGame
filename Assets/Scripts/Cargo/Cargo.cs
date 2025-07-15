using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    [Header("Properties")]
    public string cargoName;
    [Range(1f, 15f)] public float weight;

    public float baseValue;
    public float CurrentValue => baseValue * (1f - damagePercent);

    [Header("Damage")]
    private float damagePercent = 0;
    [SerializeField] private float minImpact = 5f;
    [Range(0f, 0.01f)][SerializeField] private float damageMultiplier = 0.001f;

    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;

        if (impactAmount > minImpact)
        {
            damagePercent += ((impactAmount - minImpact) * damageMultiplier);
            damagePercent = Mathf.Clamp01(damagePercent);

            Debug.Log($"{cargoName} hit for {impactAmount} force: damage now at {damagePercent * 100f:F1}% and value reduced to ${CurrentValue}");
        }
    }

   /* private void Update()
    {
        Debug.Log($"{cargoName} damage:{damagePercent * 100f:F1}% and value: ${CurrentValue}");
    }*/
}
