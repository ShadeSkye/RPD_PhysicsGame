/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
*//*
public enum CargoType
{
    Any,
    Crate,
    Bomb
}*//*

[RequireComponent(typeof(Damageable), typeof(AudioSource))]
public class OLDCargo : GravityBody
{
    private AudioSource audioSource;
    protected Damageable dmg;

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
    [Range(1f, 15f)] public float weight;
    public CargoType type;

    public float baseValue;
    public float CurrentValue => baseValue * (1f - DamagePercent);


    protected override void Awake()
    {
        base.Awake();
        rb.mass = Mathf.Clamp(weight, 1f, 15f);

        ConnectReferences();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

    }
    private void ConnectReferences()
    {
        if (dmg == null) dmg = GetComponent<Damageable>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
}*/