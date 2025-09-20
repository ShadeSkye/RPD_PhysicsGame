using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : GravitySource
{
    [Header("Explosion")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float torque = 100f;

    [Header("Teleport")]
    [SerializeField] private Vector3 teleportRange = new Vector3(50f, 50f, 50f);

    private Vector3 randomLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.GetComponent<Cargo>() != null)
        {
            Teleport(other.gameObject);
            Explode(other.gameObject);
        }
    }
    private void Teleport(GameObject target)
    {
        randomLocation = new Vector3(
           Random.Range(-teleportRange.x, teleportRange.x),
           Random.Range(-teleportRange.y, teleportRange.y),
           Random.Range(-teleportRange.z, teleportRange.z)
       );

        //Debug.Log($"Teleporting {target}to {randomLocation}!");

        target.transform.position = randomLocation;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Explode(GameObject target)
    {

        if (target.CompareTag("Player")) CameraManager.Instance.OneShotShake();

        Damageable dmg = target.GetComponent<Damageable>();
        if (dmg != null)
        {
            //Debug.Log($"Applying damage {damage} to {target.name}");
            dmg.ApplyDamage(damage);
        }

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddTorque(randomTorque * torque, ForceMode.Impulse);
        }
    }


}
