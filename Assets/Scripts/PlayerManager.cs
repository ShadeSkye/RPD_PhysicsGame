using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Damageable))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private Damageable dmg;
    private PullBeam pullBeam;

    [SerializeField] private float minImpactDrop = 5f;
    public float damagePercent
    {
        get => dmg.damagePercent;
        set => dmg.damagePercent = value;
    }
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


    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;

        // if above amount then eject
        if (impactAmount >= minImpactDrop)
        {
            if (pullBeam?.HeldBody != null)
            {
                pullBeam.UnlockBody(pullBeam.HeldBody);
            }

        }

        dmg.ApplyImpact(impactAmount);

        AudioManager.Instance.PlayOneShot("Crash");

    }

}
