using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [HideInInspector] public bool isInitialised; 
    [HideInInspector] public bool isLocked;
    public string bodyName;

    [Header("Gravity Settings")]
    public Rigidbody rb;
    public bool dontPullPlayer;

    [SerializeField] private bool isGravitySource;
    [SerializeField] private float localGravity = 1f;
    public float radius = 1f;

    [Header("Orbit Settings")]
    [SerializeField] public float orbitDistance;
    [SerializeField] public GravityBody orbitTarget;

    private Vector3 initialVelocity;
    public float lastReleasedTime;

    public bool isGravityAffected => !isGravitySource && isInitialised && !isLocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isGravitySource;
        rb.useGravity = false;

        if (isGravitySource) // generate mas from gravity and radius
        {
            localGravity = localGravity == 0 ? 1f : localGravity;
            radius = radius == 0 ? 1f : radius;
            rb.mass = (localGravity * radius * radius) / GravityManager.Instance.gravitationalConstant;
            transform.localScale = Vector3.one * radius * 2;
        }
        else if (TryGetComponent<Cargo>(out Cargo cargo))
        {
            rb.mass = Mathf.Clamp(cargo.weight, 1f, 15f);
            bodyName = cargo.cargoName;
        }
    }

    private void OnValidate()
    {
        UpdateBodySize();
    }

    private void LateUpdate()
    {
        if (isInitialised) return;

        if (orbitTarget != null && !isGravitySource)
        {
            CalculateInitialVelocity();
        }

        GravityManager.Instance.RegisterBody(this);
        isInitialised = true;
    }

    private void UpdateBodySize()
    {
        // set size using radius
        radius = radius == 0 ? 1f : radius;
        transform.localScale = Vector3.one * radius * 2;
    }

    private void CalculateInitialVelocity()
    {
        Vector3 directionFromTarget = Vector3.right;

        // set initial position
        float orbitalRadius = orbitTarget.radius + orbitDistance;
        Vector3 startPosition = orbitTarget.transform.position + directionFromTarget * orbitalRadius;
        
        //transform.position = startPosition;
        rb.MovePosition(startPosition);

        //UnityEngine.Debug.Log($"Setting {this.gameObject} to position {startPosition}");

        // calculate velocity
        float velocityMagnitude = Mathf.Sqrt(GravityManager.Instance.gravitationalConstant * orbitTarget.rb.mass / orbitalRadius);
        Vector3 directionToTarget = (rb.position - orbitTarget.rb.position).normalized;

        // calculate direction
        Vector3 orbitalPlane = CalculateOrbitalPlane();
        Vector3 orbitDirection = Vector3.Cross(directionToTarget, orbitalPlane).normalized;

        // apply velocity
        rb.velocity = orbitDirection * velocityMagnitude;
    }

    private Vector3 CalculateOrbitalPlane()
    {
        float r = UnityEngine.Random.value;

        if (r <= GravityManager.Instance.alignedOrbitChance) // chance of being normal
        {
            return  Vector3.up;
        }
        else
        {
            return UnityEngine.Random.onUnitSphere;
        }
    }

    private void OnDestroy()
    {
        if (GravityManager.Instance != null)
            GravityManager.Instance.UnregisterBody(this);
    }

}