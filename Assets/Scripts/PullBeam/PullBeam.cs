using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(BoxCollider))]
public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float beamStrength = 500f;
    [SerializeField] private float maxPullSpeed = 500f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float ejectForce = 10f;

    [Header("Hold Settings")]
    [SerializeField] private Vector3 holdOffset = Vector3.zero;

    private RectTransform targetMask;
    private BoxCollider holdZone;


    private Cargo heldCargo;
    public Cargo HeldCargo => heldCargo;

    private Vector3 heldPosition;
    private Quaternion heldRotation;

    public bool isPulling;
    private float lockCooldown = 1f;

    private List<Cargo> allCargo = new List<Cargo>();
    private HashSet<Cargo> cargoInRange = new HashSet<Cargo>();

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        allCargo.AddRange(FindObjectsOfType<Cargo>());
        holdZone = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (targetMask == null && UIManager.Instance != null)
        {
            targetMask = UIManager.Instance.TargetMask;
            Debug.Log("Assigned targetMask");
        }
    }

    private void FixedUpdate()
    {
        UpdateBeamTargets();

        // Move cargo with ship
        if (heldCargo != null)
        {
            heldPosition = transform.position + transform.forward + holdOffset;
            heldCargo.rb.MovePosition(heldPosition);

            heldRotation = transform.rotation;
            heldCargo.rb.MoveRotation(heldRotation);

            if (heldCargo.rb.velocity.magnitude > maxPullSpeed)
            {
                heldCargo.rb.velocity = heldCargo.rb.velocity.normalized * maxPullSpeed;
            }
        }
    }

    private void UpdateBeamTargets()
    {
        if (targetMask == null) return;

        foreach (var c in allCargo)
        {
            if (c.IsLocked || c.CompareTag("Player")) continue;

            Vector3 screenPos = mainCam.WorldToScreenPoint(c.transform.position);

            //Debug.Log(screenPos.z);

            // Behind camera
            if (screenPos.z <= 0)
            {
                //Debug.Log($"{c} behind camera");
                cargoInRange.Remove(c);
                continue;
            }
            else
            {
                // Too far away
                if (screenPos.z > maxDistance)
                {
                    //Debug.Log($"{c} is {screenPos.z:F2}: too far away");
                    cargoInRange.Remove(c);
                    continue;
                }
                else
                {
                    Vector3 maskWorldPos = targetMask.position; // center in screen space
                    Vector2 maskSize = targetMask.rect.size * targetMask.lossyScale; // scaled size

                    float left = maskWorldPos.x - maskSize.x / 2;
                    float right = maskWorldPos.x + maskSize.x / 2;
                    float bottom = maskWorldPos.y - maskSize.y / 2;
                    float top = maskWorldPos.y + maskSize.y / 2;

                    bool inside = screenPos.x >= left && screenPos.x <= right &&
                                  screenPos.y >= bottom && screenPos.y <= top;

                    if (inside)
                    {
                        //Debug.Log($"Cargo visually within targetMask: {c.name} | Distance: {screenPos.z:F2}");
                        cargoInRange.Add(c);
                        ApplyPull(c);
                    }
                    else
                    {
                        cargoInRange.Remove(c);
                    }
                }
                    
            }

        }

        UpdateCargoVisuals();
    }


    private void ApplyPull(Cargo target)
    {
        // if is currently pullable
        if (isPulling && cargoInRange.Contains(target))
        {
            // if is a valid object to be pulled
            if (target == null || target.IsLocked || target.CompareTag("Player")) return;

            Vector3 offset = holdZone.transform.position - target.rb.position;
            float distance = Mathf.Max(offset.magnitude, 0.1f);

            float forceMagnitude = beamStrength * Mathf.Clamp01(1f - (distance / maxDistance));
            target.rb.AddForce(offset.normalized * forceMagnitude * target.rb.mass);

            Debug.Log($"Pulling {target} Distance:{distance}");
        }
    }

    public void LockCargo(Cargo c)
    {

        if (heldCargo != null) return;
        if (Time.time - c.LastReleasedTime < lockCooldown) return;

        c.transform.SetParent(transform);
        c.rb.velocity = Vector3.zero;
        c.rb.angularVelocity = Vector3.zero;

        c.IsLocked = true;
        c.rb.isKinematic = false;

        GravityManager.Instance.UnregisterObject(c);

        heldCargo = c;
        cargoInRange.Remove(c);

        AudioManager.Instance.PlayOneShot("Lock");
        CarryingDisplay.Instance.SetCarrying(c);
    }

    public void UnlockCargo(Cargo c)
    {
        if (c == null) return;

        c.LastReleasedTime = Time.time;
        c.IsLocked = false;
        c.rb.isKinematic = false;
        c.transform.SetParent(null);

        GravityManager.Instance.RegisterObject(c);

        heldCargo = null;

        AudioManager.Instance.PlayOneShot("Eject");
        CarryingDisplay.Instance.ClearCarrying();
    }

    public void EjectCargo(Cargo c)
    {
        if (c == null || c.rb == null) return;

        UnlockCargo(c);
        c.rb.AddForce(transform.forward * ejectForce, ForceMode.Impulse);
    }

    private void UpdateCargoVisuals()
    {
        UIManager.Instance.CargoInRange(cargoInRange.Count > 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter fired with {other.name}");
        Cargo cargo = other.GetComponent<Cargo>();
        if (cargo != null)
        {
            Debug.Log($"Cargo {cargo} entered HoldZone");
            LockCargo(cargo);
        }
    }
}
