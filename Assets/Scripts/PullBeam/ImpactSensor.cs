using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ImpactSensor : MonoBehaviour
{
    PullBeam pullBeam;
    [SerializeField] private float minImpact = 5f;

    private void Awake()
    {
        pullBeam = GetComponentInChildren<PullBeam>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // get hit amount
        float impactAmount = collision.relativeVelocity.magnitude;
        //Debug.LogWarning($"Hit {collision.gameObject} for {impactAmount} force");

        // if above amount then eject
        if (impactAmount >= minImpact && pullBeam?.HeldBody != null)
        {
            pullBeam.UnlockBody(pullBeam.HeldBody);
        }
    }
}
