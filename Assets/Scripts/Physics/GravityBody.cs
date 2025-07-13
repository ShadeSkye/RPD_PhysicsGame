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
    public GravityBody orbitTarget;
    public Vector3 initialVelocity;

    //public Vector3 velocity;

    private void OnValidate()
    {
        UpdateBodySize();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isGravitySource;
        rb.useGravity = false;

        GravityManager.Instance.RegisterBody(this);

        // set size
        UpdateBodySize();

        if (gameObject.CompareTag("Player")) return;

        // set mass using local gravity and radius
        localGravity = localGravity == 0 ? 1f : localGravity;
        rb.mass = (localGravity * radius * radius) / GravityManager.Instance.gravitationalConstant;

        if (isGravitySource) return;

        rb.velocity = initialVelocity;
    }

    private void UpdateBodySize()
    {
        // set size using radius
        radius = radius == 0 ? 1f : radius;
        transform.localScale = Vector3.one * radius * 2;
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