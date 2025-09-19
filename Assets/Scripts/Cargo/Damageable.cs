using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("Damage")]
    public float damagePercent = 0;
    public float minImpact = 5f;
    [Range(0f, 0.01f)]public float damageMultiplier = 0.001f;

    [Header("Sound")]
    public string hitSFX = "CrateHit";

    public void ApplyImpact(float impactAmount)
    {
        if (impactAmount > minImpact)
        {
            damagePercent += ((impactAmount - minImpact) * damageMultiplier);
            damagePercent = Mathf.Clamp01(damagePercent);
        }
    }

    public void ApplyDamage(float damageAmount)
    {
        damagePercent += damageAmount / 100f;
        damagePercent = Mathf.Clamp01(damagePercent);
    }
}
