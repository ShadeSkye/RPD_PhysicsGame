using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private Damageable dmg;
    private PullBeam pullBeam;

    [SerializeField] private float minImpactDrop = 1f;
    public float damagePercent
    {
        get => dmg.damagePercent;
        set => dmg.damagePercent = value;
    }

    private void OnEnable() => SceneManager.sceneLoaded += (_, __) => OnSceneLoaded();
    private void OnDisable() => SceneManager.sceneLoaded -= (_, __) => OnSceneLoaded();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dmg = GetComponent<Damageable>();
        pullBeam = GetComponentInChildren<PullBeam>();

    }

    void OnSceneLoaded()
    {
        dmg.damagePercent = 0;
        ResetLocation(LevelManager.Instance.SpaceStation.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LevelBounds") || collision.gameObject.CompareTag("Cargo")) return;

        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;

        // if above amount then eject
        if (impactAmount >= (minImpactDrop))
        {
            if (pullBeam?.HeldCargo != null)
            {
                pullBeam.UnlockCargo(pullBeam.HeldCargo);
            }

        }

        dmg.ApplyImpact(impactAmount);

        AudioManager.Instance.PlayOneShot("Crash");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hangar"))
        {
            Debug.Log("Entered hangar");

            UIManager.Instance.OpenHangar();
        }
    }

    public void ResetLocation(Transform station)
    {
        //Debug.LogError($"PLAYERMANAGER: Setting location to {station.position}");

        Vector3 positionOffset = new Vector3(56.7f, -9.1f, 0);
        Vector3 rotationOffset = new Vector3(0, -90, 0);

        transform.position = station.position + positionOffset;
        transform.rotation = station.rotation * Quaternion.Euler(rotationOffset);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = new Vector3(-10f, 0, 0);
            rb.angularVelocity = Vector3.zero;  
        }
    }
}
