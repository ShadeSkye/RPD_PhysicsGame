using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SolarBattery : Bomb
{
    [Header("Velocity Trigger")]
    [SerializeField] private float maxVelocity = 60f;
    [SerializeField] private float maxAngularVelocity = 7f;
    private void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float velocity = rb.velocity.magnitude;
        float angularVelocity = rb.angularVelocity.magnitude;

        Debug.Log($"Velocity: {velocity}/{maxVelocity} Angular velocity: {angularVelocity}/{maxAngularVelocity}");

        if (velocity > maxVelocity || angularVelocity > maxAngularVelocity)
        {
            Explode();
        }

    }
}