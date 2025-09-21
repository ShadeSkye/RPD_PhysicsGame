using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Cargo
{
    [Header("Trigger")]

    [SerializeField] private float impactDamage = 0.05f;
    [SerializeField] private float maxSafeSpeed = 300f;

    [Header("Explosion")]
    [SerializeField] private float radius = 10f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float force = 2500f;
    [SerializeField] private float torque = 100f;

    private Vector3 center;

    protected override void Awake()
    {
        base.Awake();

        type = CargoType.Bomb;
    }

    private void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float speed = rb.velocity.magnitude;

        if (speed > maxSafeSpeed)
        {
            Explode();
        }
    }

    protected override void CollisionDamage(Collision collision)
    {
        Debug.Log(DamagePercent);
        if (DamagePercent > impactDamage) Explode();

    }
    protected void Explode()
    {
        if(CarryingDisplay.Instance.CurrentCargo == this)
            CarryingDisplay.Instance.ClearCarrying(); 

        center = transform.position;

        Debug.Log("EXPLODE");
        Collider[] objectsInRange = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in objectsInRange)
        {
            if (hit.CompareTag("Player")) CameraManager.Instance.OneShotShake();

            Damageable dmg = hit.GetComponent<Damageable>();
            if (dmg != null)
            {
                dmg.ApplyDamageWithResistance(damage, DamageType.Impact);
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

        Destroy(gameObject);
    }
}