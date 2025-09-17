using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AcidGas : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float interval = 0.5f;

    private List<Damageable> targets = new List<Damageable>();

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
    private void OnEnable()
    {
        StartCoroutine(DealDamage());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        targets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Damageable dmg) && !targets.Contains(dmg))
        {
            targets.Add(dmg);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Damageable dmg))
        {
            targets.Remove(dmg);
        }
    }

    private IEnumerator DealDamage()
    {
        while (true)
        {
            foreach (var t in targets)  
            {
                if (t == null) continue;

                t.damagePercent += (damage * interval) / 100f;
                t.damagePercent = Mathf.Clamp01(t.damagePercent);

                Debug.Log($"Applied {damage} damage to target {t}");
            }

            if (CarryingDisplay.Instance != null && CarryingDisplay.Instance.CurrentCargo != null && targets.Contains(CarryingDisplay.Instance.CurrentCargo.GetComponent<Damageable>()))
            {
                CarryingDisplay.Instance.UpdateCarrying();
            }

            yield return new WaitForSeconds(interval);
        }

    }
}
