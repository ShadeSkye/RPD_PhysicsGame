using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum CargoType
{
    Any,
    Crate,
    Bomb,
    Egg
}

[RequireComponent(typeof(Damageable), typeof(AudioSource))]
public class Cargo : GravityBody
{
    private AudioSource audioSource;
    protected Damageable dmg;

    [HideInInspector] 
    public bool IsLocked;
    [HideInInspector]
    public float LastReleasedTime;

    [HideInInspector]
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

    public CargoType type;

    public Vector2 WeightRange;
    public float weight;

    public Vector2 ValueRange;
    public float baseValue;
    public float CurrentValue => baseValue * (1f - DamagePercent);

    protected override void Awake()
    {
        base.Awake();

        weight = Mathf.RoundToInt(Random.Range(WeightRange.x, WeightRange.y));
        rb.mass = weight;

        ConnectReferences();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        baseValue = Mathf.RoundToInt(Random.Range(ValueRange.x, ValueRange.y));

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
            CurrencyManager.Instance.AddEarnings(CurrentValue);
            CarryingDisplay.Instance.ClearCarrying();

            AudioManager.Instance.PlayOneShot("Deposited");

            GravityManager.Instance.UnregisterObject(this);
            LevelManager.Instance?.UnregisterCargo(this);

            LevelManager.Instance?.OnCargoDelivered(this);

            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (dmg == null) return;

        if (collision.gameObject.CompareTag("LevelBounds"))
        {
            LevelBounds.Instance.Teleport(this.gameObject);
        }
        else
        {
            float impactAmount = collision.relativeVelocity.magnitude;
            dmg.ApplyImpact(impactAmount);

            CollisionDamage(collision);
        }

    }

    protected virtual void CollisionDamage(Collision collision)
    {

        if (AudioManager.Instance.audioLookup.TryGetValue("CrateHit", out var sound))
        {
            audioSource.PlayOneShot(sound.clip);
        }
    }
}