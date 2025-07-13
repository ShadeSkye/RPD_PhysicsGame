using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
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

    private void Start()
    {
        GravityManager.Instance.RegisterBody(this);

        if (!isGravitySource && !gameObject.CompareTag("Player") && orbitTarget != null)
        {
            CalculateInitialVelocity();
        }
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
        float totalDistance = orbitTarget.radius + orbitDistance;
        Vector3 startPosition = orbitTarget.transform.position + directionFromTarget * totalDistance;
        transform.position = startPosition;

        // calculate orbital radius
        float orbitalRadius = Vector3.Distance(rb.transform.position, orbitTarget.rb.transform.position);

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