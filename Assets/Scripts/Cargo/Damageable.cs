using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Acid, Impact }


public class Damageable : MonoBehaviour
{
    [Header("Damage")]
    public float damagePercent = 0;
    public float minImpact = 5f;
    public float damageMultiplier = 0.5f; // base fragility

    [Header("Sound")]
    public string hitSFX = "CrateHit";

    public void ApplyImpact(float impactAmount)
    {
        if (impactAmount > minImpact)
        {
            float impactDamage = ((impactAmount - minImpact) * damageMultiplier);
            ApplyDamageWithResistance(impactDamage, DamageType.Impact);
        }
    }

    public void ApplyDamage(float damageAmount)
    {
        damagePercent += damageAmount / 100f;
        damagePercent = Mathf.Clamp01(damagePercent);
    }

    public void ApplyDamageWithResistance(float damageAmount, DamageType type)
    {
        float multiplier = 1f;

        if (gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case DamageType.Impact:
                    multiplier = 1f - ShipManager.Instance.ImpactDamageResistance;
                    break;
                case DamageType.Acid:
                    multiplier = 1f - ShipManager.Instance.AcidDamageResistance;
                    break;
            }
        }

        ApplyDamage(multiplier * damageAmount);
        //Debug.Log($"Applied {multiplier * damageAmount} {type} damage to {gameObject.name}");
    }
}
