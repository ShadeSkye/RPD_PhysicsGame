using System;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [HideInInspector] public bool isInitialised;

    [Header("Gravity Settings")]
    public Rigidbody rb;

    [SerializeField] private bool isGravitySource;
    [SerializeField] private float localGravity = 1f;
    public float radius = 1f;

    [Header("Orbit Settings")]
    [SerializeField] private float orbitDistance;
    [SerializeField] private GravityBody orbitTarget;

    private Vector3 initialVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isGravitySource;
        rb.useGravity = false;

        if (!gameObject.CompareTag("Player"))
        {
            localGravity = localGravity == 0 ? 1f : localGravity;
            radius = radius == 0 ? 1f : radius;
            rb.mass = (localGravity * radius * radius) / GravityManager.Instance.gravitationalConstant;
            transform.localScale = Vector3.one * radius * 2;
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
        Vector3 orbitDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized;

        // apply velocity
        rb.velocity = orbitDirection * velocityMagnitude;
    }

    private void OnDestroy()
    {
        if (GravityManager.Instance != null)
            GravityManager.Instance.UnregisterBody(this);
    }



    /*

    public void UpdateVelocity(Vector3 acceleration)
    {
        velocity += acceleration * Time.fixedDeltaTime;
    }

    public void UpdatePosition()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

    }*/
}