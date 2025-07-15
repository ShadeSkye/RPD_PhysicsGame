using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType
{
    Pull,
    Hold,
}

[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour
{
    PullBeam pullBeam;
    public ZoneType zoneType;

    private void Awake()
    {
        pullBeam = GetComponentInParent<PullBeam>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zoneType != ZoneType.Hold) return;

        GravityBody body = other.GetComponent<GravityBody>();
        if (body == null) return;

        pullBeam.LockBody(body);
    }

    private void OnTriggerStay(Collider other)
    {
        if (zoneType != ZoneType.Pull) return;

        GravityBody body = other.GetComponent<GravityBody>();
        if (body == null) return;

        pullBeam.ApplyPull(body);
    }

}
