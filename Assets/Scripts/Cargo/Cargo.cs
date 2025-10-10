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
    Egg,
    SolarBattery
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

    public Vector2 WeightRange = new Vector2(1,15);
    public Vector2 ValueRange = new Vector2(10, 150);

    private float baseValue;
    private float weight;
    public float CurrentValue => baseValue * (1f - DamagePercent);

    protected override void Awake()
    {
        base.Awake();

        weight = Mathf.RoundToInt(Random.Range(WeightRange.x, WeightRange.y));
        rb.mass = weight;
        weight = Random.Range(WeightRange.x, WeightRange.y);
        rb.mass = weight;
        
        baseValue = Mathf.RoundToInt(Random.Range(ValueRange.x, ValueRange.y));

        ConnectReferences();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Alpha3)) dmg.Remove(this);
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
            dmg.Remove(this);

            CurrencyManager.Instance.AddEarnings(CurrentValue);
            CarryingDisplay.Instance.ClearCarrying();

            AudioManager.Instance.PlayOneShot("Deposited");

            GravityManager.Instance.UnregisterObject(this);
            LevelManager.Instance?.UnregisterCargo(this);
            
        }
    }

    public void OnDeliver()
    {
        LevelManager.Instance?.OnCargoDelivered(this);

        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (dmg == null) return;

        if (collision.gameObject.CompareTag("LevelBounds"))
        {
            dmg.Respawn();
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