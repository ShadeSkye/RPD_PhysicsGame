using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float damage = 25f;

    // random teleport


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Damageable dmg = other.GetComponent<Damageable>();
            dmg.ApplyDamage(damage);

            Destroy(gameObject);
        }
    }
}
