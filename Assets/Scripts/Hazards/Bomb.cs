using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Cargo
{
    [Header("Trigger")]

    [SerializeField] private float impactDamage = 0.05f;

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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioManager.Instance.PlayPositional("BombExplosion", this.transform.position);
        }
    }

    protected override void CollisionDamage(Collision collision)
    {
        if (DamagePercent > impactDamage)
        {

            //Debug.Log($"damage {DamagePercent} more than threshold {impactDamage}");
            Explode();
        }
        else
        {
            //Debug.Log($"damage {DamagePercent} less than threshold {impactDamage}");
        }

    }
    protected void Explode()
    {

        if(CarryingDisplay.Instance.CurrentCargo == this)
            CarryingDisplay.Instance.ClearCarrying(); 

        center = transform.position;

        ParticleManager.Instance.PlayExplosion(center);

        AudioManager.Instance.PlayPositional("BombExplosion", this.transform.position);

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