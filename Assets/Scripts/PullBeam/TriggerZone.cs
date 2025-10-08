/*using System.Collections;
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
        if (other.TryGetComponent<Cargo>(out Cargo c))
        {
            if (zoneType == ZoneType.Hold)
                pullBeam.OnHoldZoneEnter(c);
            else
                pullBeam.OnPullZoneEnter(c);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Cargo>(out Cargo c))
        {
            if (zoneType == ZoneType.Pull)
                pullBeam.OnPullZoneStay(c);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Cargo>(out Cargo c))
        {
            if (zoneType == ZoneType.Pull)
                pullBeam.OnPullZoneExit(c);
        }
    }


}
*/