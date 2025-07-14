using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    /*[SerializeField] private float maxBeamDistance;
    [SerializeField, Range(0f, 1f)] private float viewWidth = 0.95f;*/
    [SerializeField, Range(0f, 100f)] private float beamStrength = 100f;
    [SerializeField, Range(0f, 100f)] private float lockStrength = 500f;

    [Header("Zone Settings")]
    [SerializeField] private Collider pullZone;
    [SerializeField] private Collider holdZone;

    private List<GravityBody> bodiesInBeam = new List<GravityBody>();

    private GravityBody heldBody;

    private void FixedUpdate()
    {

        if (heldBody != null)
        {
            heldBody.transform.localPosition = Vector3.zero; 
        }
    }
    public void ApplyPull(GravityBody target)
    {
        if (target != null && target.isGravityAffected && !target.CompareTag("Player"))
        {
                Vector3 offset = transform.position - target.rb.position;
                float distance = offset.magnitude;

                if (distance <= 0)
                {
                    distance = 0.1f;
                }

                float forceMagnitude = GravityManager.Instance.gravitationalConstant * ((beamStrength * 100000) * target.rb.mass) / (distance * distance);

                Vector3 direction = offset.normalized;

                target.rb.AddForce(direction * forceMagnitude);

        }
        
    }

    internal void LockBody(GravityBody body)
    {
        if (body != null && body.isGravityAffected && !body.CompareTag("Player"))
        {
            Debug.Log($"{body.name} entered hold zone");

            // stop moving
            body.isLocked = true;
            body.rb.velocity = Vector3.zero;
            body.rb.angularVelocity = Vector3.zero;

            body.rb.isKinematic = true;

            GravityManager.Instance.UnregisterBody(body);

            // parent to ship
            body.transform.SetParent(holdZone.transform);

            if (heldBody == null)
            {
                heldBody = body;
            }
        }
    }

}
