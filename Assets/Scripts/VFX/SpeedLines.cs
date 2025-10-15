using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem ps;

    [SerializeField] private Vector2 speedRange;
    [SerializeField] private float maxEmission;

    private void Update()
    {
        Vector3 velocity = rb.velocity;
        float speed = velocity.magnitude;

        var emission = ps.emission;
        emission.rateOverTime = Mathf.Lerp(0, maxEmission, Mathf.Clamp01((speed - speedRange.x) / (speedRange.y - speedRange.x)));

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-velocity.normalized, transform.up);
        }

    }
}
