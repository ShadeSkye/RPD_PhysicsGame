using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum CargoType
{
    Any,
    Crate,
    Bomb
}

[RequireComponent(typeof(Damageable), typeof(AudioSource), typeof(LookAtTarget))]
public class Cargo : MonoBehaviour
{
    private AudioSource audioSource;
    protected Damageable dmg;
    private LookAtTarget lookAt;

    public float DamagePercent
    {
        get
        {
            ConnectReferences();
            return dmg ? dmg.damagePercent : 0f;
        }
        set
        {
            ConnectReferences();
            if (dmg) dmg.damagePercent = value;
        }
    }

    [Header("Properties")]
    public string cargoName;
    [Range(1f, 15f)] public float weight;
    public CargoType type;

    public float baseValue;
    public float CurrentValue => baseValue * (1f - DamagePercent);


    private void Awake()
    {
        ConnectReferences();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        lookAt.displayName = cargoName;

    }
    private void ConnectReferences()
    {
        if (dmg == null) dmg = GetComponent<Damageable>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (lookAt == null) lookAt = GetComponent<LookAtTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Depot"))
        {
            CarryingDisplay.Instance.totalEarnings += CurrentValue;
            CarryingDisplay.Instance.UpdateEarnings();
            CarryingDisplay.Instance.ClearCarrying();

            AudioManager.Instance.PlayOneShot("Deposited");

            if(LevelManager.Instance != null) LevelManager.Instance.OnCargoDelivered(this);
            
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (dmg == null) return;

        float impactAmount = collision.relativeVelocity.magnitude;
        dmg.ApplyImpact(impactAmount);

        CollisionDamage(collision);
    }

    protected virtual void CollisionDamage(Collision collision)
    {

        if (AudioManager.Instance.audioLookup.TryGetValue("CrateHit", out var sound))
        {
            audioSource.PlayOneShot(sound.clip);
        }
    }
}