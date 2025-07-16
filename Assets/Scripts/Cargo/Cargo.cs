using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cargo : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Properties")]
    public string cargoName;
    [Range(1f, 15f)] public float weight;

    public float baseValue;
    public float CurrentValue => baseValue * (1f - damagePercent);

    [Header("Damage")]
    public float damagePercent = 0;
    [SerializeField] private float minImpact = 5f;
    [Range(0f, 0.01f)][SerializeField] private float damageMultiplier = 0.001f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 15f;
        audioSource.volume = 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;

        if (impactAmount > minImpact)
        {
            damagePercent += ((impactAmount - minImpact) * damageMultiplier);
            damagePercent = Mathf.Clamp01(damagePercent);

            AudioManager.Instance.PlaySFX(OneShotSFX.CrateHit);
            Debug.Log($"{cargoName} hit for {impactAmount} force: damage now at {damagePercent * 100f:F1}% and value reduced to ${CurrentValue}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Depot"))
        {
            CarryingDisplay.Instance.totalEarnings += CurrentValue;
            CarryingDisplay.Instance.UpdateEarnings();
            CarryingDisplay.Instance.ClearCarrying();

            AudioManager.Instance.PlaySFX(OneShotSFX.Deposited);
            Destroy(gameObject);
        }
    }

    public void PlayCrateHitSound()
    {
        AudioClip clip = AudioManager.Instance.oneShotSFX[(int)OneShotSFX.CrateHit];
        audioSource.PlayOneShot(clip);
    }
}
