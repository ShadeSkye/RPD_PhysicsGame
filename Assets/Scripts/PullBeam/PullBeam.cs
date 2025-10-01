using System.Collections.Generic;
using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float beamStrength = 500;
    [SerializeField] private float maxPullSpeed = 500;

    [Header("Zone Settings")]
    [SerializeField] private Collider pullZone;
    [SerializeField] private Collider holdZone;

    [Header("Hold Settings")]
    private Cargo heldCargo;
    public Cargo HeldCargo => heldCargo;
    [SerializeField] private float ejectForce;
    [SerializeField] private Vector3 holdOffset = Vector3.zero;

    private Vector3 heldPosition;
    private Quaternion heldRotation;

    public bool isPulling;
    private float lockCooldown = 1f;
    private List<Cargo> cargoInBeam = new List<Cargo>();

    private void FixedUpdate()
    {
        if (heldCargo != null)
        {
            heldPosition = holdZone.transform.TransformPoint(holdOffset);
            heldCargo.rb.MovePosition(heldPosition);
            heldRotation = holdZone.transform.rotation;
            heldCargo.rb.MoveRotation(heldRotation);

            if (heldCargo.rb.velocity.magnitude > maxPullSpeed)
            {
                heldCargo.rb.velocity = heldCargo.rb.velocity.normalized * maxPullSpeed;
            }
        }
    }

    public void ApplyPull(Cargo target)
    {
        if (isPulling)
        {
            //Debug.Log($"Attempting pull {target}");

            if (target != null && !target.IsLocked && !target.CompareTag("Player"))
            {
                //Debug.Log($"Successful pull {target}");

                if (!cargoInBeam.Contains(target)) cargoInBeam.Add(target);

                Vector3 offset = transform.position - target.rb.position;
                float distance = offset.magnitude;

                if (distance <= 0) distance = 0.1f;

                float forceMagnitude = GravityManager.Instance.gravitationalConstant * ((beamStrength * 1000) * target.rb.mass) / (distance * distance);

                Vector3 direction = offset.normalized;

                target.rb.AddForce(direction * forceMagnitude);


            }

        }
    }

    internal void LockCargo(Cargo c)
    {
        if (!cargoInBeam.Contains(c)) return;

        if (c != null && !c.IsLocked && !c.CompareTag("Player") && heldCargo == null)
        {
            if (Time.time - c.LastReleasedTime < lockCooldown) return;

            c.rb.velocity = Vector3.zero;
            c.rb.angularVelocity = Vector3.zero;

            c.IsLocked = true;
            c.rb.isKinematic = true;

            GravityManager.Instance.UnregisterObject(c);

            c.transform.SetParent(holdZone.transform);


            heldCargo = c;

            cargoInBeam.Remove(c);
            Debug.Log($"Picked up {c}");
            Debug.Log(c.DamagePercent);
            Debug.Log(PlayerManager.Instance.damagePercent);
            AudioManager.Instance.PlayOneShot("Lock");

            CarryingDisplay.Instance.SetCarrying(c);
        }
    }

    public void UnlockCargo(Cargo c)
    {
        if (c != null)
        {
            c.LastReleasedTime = Time.time;

            c.IsLocked = false;
            c.rb.isKinematic = false;
            c.transform.SetParent(null);

            GravityManager.Instance.RegisterObject(c);

            heldCargo = null;

            AudioManager.Instance.PlayOneShot("Eject");
            CarryingDisplay.Instance.ClearCarrying();
        }
    }

    public void EjectCargo(Cargo c)
    {
        Debug.Log($"Ejected {c}");

        if (c.rb == null) return;

        UnlockCargo(c);
        c.rb.AddForce(transform.forward * ejectForce, ForceMode.Impulse);
    }

    public void OnPullZoneEnter(Cargo c)
    {
        if (!cargoInBeam.Contains(c))
        {
            cargoInBeam.Add(c);
            UpdateCargoVisuals();
        }
    }

    public void OnPullZoneStay(Cargo c)
    {
        ApplyPull(c);    
    }

    public void OnPullZoneExit(Cargo c)
    {
        cargoInBeam.Remove(c);
        UpdateCargoVisuals();
    }

    public void OnHoldZoneEnter(Cargo c)
    {
        LockCargo(c);
    }

    private void UpdateCargoVisuals()
    {
        //UIManager.Instance.CargoInRange(cargoInBeam.Count > 0);
    }

}