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

        switch (zoneType)
        {
            case ZoneType.Pull:
                pullBeam.ApplyPull(body);
                Debug.Log($"Pulling {body}");
                break;

            case ZoneType.Hold:
                pullBeam.LockBody(body);
                Debug.Log($"Holding {body}");
                break;

        }
    }
}
