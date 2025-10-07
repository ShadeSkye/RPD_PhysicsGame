using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : GravitySource
{
    [Header("Explosion")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float torque = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.GetComponent<Cargo>() != null)
        {
            LevelBounds.Instance.Teleport(other.gameObject);
            Explode(other.gameObject);
        }
    }

    private void Explode(GameObject target)
    {

        if (target.CompareTag("Player")) CameraManager.Instance.OneShotShake();

        Damageable dmg = target.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.ApplyDamageWithResistance(damage, DamageType.Impact);
        }

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddTorque(randomTorque * torque, ForceMode.Impulse);
        }
    }


}
