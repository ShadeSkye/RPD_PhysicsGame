using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    public bool isGravitySource;
    public Vector3 initialVelocity;

    //public Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isGravitySource;
        rb.useGravity = false;

        rb.velocity = initialVelocity;

        GravityManager.Instance.RegisterBody(this);
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
