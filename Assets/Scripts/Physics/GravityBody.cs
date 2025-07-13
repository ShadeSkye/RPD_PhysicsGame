using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [Header("Gravity Settings")]
    [HideInInspector] public Rigidbody rb;
    public bool isGravitySource;
    public float localGravity;
    public float radius;

    [Header("Orbit Settings")]
    public float orbitDistance;
    public GravityBody orbitTarget;
    public Vector3 initialVelocity;

    //public Vector3 velocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isGravitySource;
        rb.useGravity = false;
    }

    private void OnValidate()
    {
        UpdateBodySize();
    }

    private void Start()
    {

        GravityManager.Instance.RegisterBody(this);

        // set size
        UpdateBodySize();

        if (gameObject.CompareTag("Player")) return;

        // set mass using local gravity and radius
        localGravity = localGravity == 0 ? 1f : localGravity;
        rb.mass = (localGravity * radius * radius) / GravityManager.Instance.gravitationalConstant;

        if (isGravitySource) return;

        if (orbitTarget != null) CalculateInitialVelocity();
        rb.velocity = initialVelocity;
    }

    private void UpdateBodySize()
    {
        // set size using radius
        radius = radius == 0 ? 1f : radius;
        transform.localScale = Vector3.one * radius * 2;
    }

    private void CalculateInitialVelocity()
    {
        float orbitalRadius = Vector3.Distance(rb.transform.position, orbitTarget.rb.transform.position) + orbitDistance;

        float velocityMagnitude = Mathf.Sqrt(GravityManager.Instance.gravitationalConstant * orbitTarget.rb.mass / orbitalRadius);
        Vector3 directionToTarget = (rb.position - orbitTarget.rb.position).normalized;
        Vector3 orbitDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized;

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