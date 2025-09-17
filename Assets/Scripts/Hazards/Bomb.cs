using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Cargo
{
    [Header("Explosion")]
    [SerializeField] private float radius = 10f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float force = 2500f;
    [SerializeField] private float torque = 100f;

    private Vector3 center;
    protected void Explode()
    {
        center = transform.position;

        Debug.Log("EXPLODE");
        Collider[] objectsInRange = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in objectsInRange)
        {
            Damageable dmg = hit.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.ApplyDamage(damage);
            }

            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector3 direction = (hit.transform.position - center).normalized;
                rb.AddForce(direction * force);

                Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.AddTorque(randomTorque * torque, ForceMode.Impulse);
            }
        }
    }
}