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
        GravityBody body = other.GetComponent<GravityBody>();
        if (body == null) return;

        if (zoneType == ZoneType.Hold)
        {

            pullBeam.OnHoldZoneEnter(body);
        }
        else
        {
            pullBeam.OnPullZoneEnter(body);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (zoneType != ZoneType.Pull) return;

        GravityBody body = other.GetComponent<GravityBody>();
        if (body == null) return;

        pullBeam.OnPullZoneStay(body);
    }

    private void OnTriggerExit(Collider other)
    {
        if (zoneType != ZoneType.Pull) return;

        GravityBody body = other.GetComponent<GravityBody>();
        if (body == null) return;

        pullBeam.OnPullZoneExit(body);
    }


}
