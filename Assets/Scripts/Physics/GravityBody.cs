using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : GravityObject
{
    [HideInInspector] public bool IsInitialised;

    [Header("Orbit Settings")]
    public float OrbitDistance;
    public GravitySource OrbitTarget;
    protected override void Awake()
    {
        base.Awake();
        rb.isKinematic = false;

    }

    private void LateUpdate()
    {
        if (IsInitialised) return;

        if (OrbitTarget != null)
        {
            CalculateInitialVelocity();
        }

        GravityManager.Instance.RegisterObject(this);
        IsInitialised = true;
    }
    private void CalculateInitialVelocity()
    {
        Vector3 directionFromTarget = Vector3.right;

        // set initial position
        float orbitalRadius = OrbitTarget.Radius + OrbitDistance;
        Vector3 startPosition = OrbitTarget.transform.position + directionFromTarget * orbitalRadius;

        //transform.position = startPosition;
        rb.MovePosition(startPosition);

        // calculate velocity
        float velocityMagnitude = Mathf.Sqrt(GravityManager.Instance.gravitationalConstant * OrbitTarget.rb.mass / orbitalRadius);
        Vector3 directionToTarget = (rb.position - OrbitTarget.rb.position).normalized;

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
            return Vector3.up;
        }
        else
        {
            return UnityEngine.Random.onUnitSphere;
        }
    }
    private void OnDestroy()
    {
        if (GravityManager.Instance != null)
            GravityManager.Instance.UnregisterObject(this);
    }
}
