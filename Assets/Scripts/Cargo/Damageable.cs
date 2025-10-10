using System.Collections;
using System.Collections.Generic;
using System;
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

    [Header("Dissolve")]
    [SerializeField] private Dissolve dissolve;

    public void ApplyImpact(float impactAmount)
    {
        //Debug.Log($"Impact on {gameObject}: {impactAmount} vs threshold {minImpact}");

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

        CheckHealth();
    }

    public void ApplyDamageWithResistance(float damageAmount, DamageType type)
    {
        Cargo cargo = GetComponent<Cargo>();
        if (cargo != null && cargo.type == CargoType.Egg && type == DamageType.Acid)
        {
            //Debug.Log($"Acid damage ignored for Egg cargo on {gameObject.name}");
            return;
        }

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

    private void CheckHealth()
    {
        //Debug.Log(damagePercent + gameObject.name);

        if(damagePercent >= 1)
        {
            if (gameObject.CompareTag("Player"))
            {
                GameManager.Instance.RestartLevel();
            }

            else
            {
                //SafeDestroy();
                Respawn();
            }
        }
    }

    private void SafeDestroy()
    {
        GravityObject g = gameObject.GetComponent<GravityObject>();
        if (g != null)
        {
            GravityManager.Instance?.UnregisterObject(g);
        }

        Cargo c = gameObject.GetComponent<Cargo>();
        if (c != null)
        {
            LevelManager.Instance?.UnregisterCargo(c);
        }

        Destroy(gameObject, 1f);
    }

    public void Respawn()
    {
        StartCoroutine(DissolveRoutine(() =>
        {
            LevelBounds.Instance.Teleport(this.gameObject);
            damagePercent = 0f;

            dissolve.ResetMaterials();
        }));
    }

    public void Remove(Cargo cargo)
    {
        StartCoroutine(DissolveRoutine(() =>
        {
            cargo.OnDeliver();
        }));
    }

    private IEnumerator DissolveRoutine(Action onComplete)
    {
        dissolve.DoEffect();

        yield return new WaitForSeconds(dissolve.Duration);

        onComplete?.Invoke();
    }

}
