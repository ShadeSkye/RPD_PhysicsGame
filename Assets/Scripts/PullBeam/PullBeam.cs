using System.Collections.Generic;
using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float beamStrength = 500;
    [SerializeField] private float maxPullSpeed = 500;

    [Header("Zone Settings")]
    [SerializeField] private Collider pullZone;
    [SerializeField] private Collider holdZone;

    [Header("Hold Settings")]
    private GravityBody heldBody;
    public GravityBody HeldBody => heldBody;
    [SerializeField] private float radiusPadding = 1f;
    [SerializeField] private float ejectForce;

    private Vector3 heldPosition;
    private Quaternion heldRotation;

    public bool isPulling;
    private float lockCooldown = 1f;
    private List<GravityBody> bodiesInBeam = new List<GravityBody>();

    private void FixedUpdate()
    {
        if (heldBody != null)
        {
            heldPosition = holdZone.transform.position + holdZone.transform.forward * (heldBody.radius + radiusPadding);
            heldBody.rb.MovePosition(heldPosition);
            heldRotation = holdZone.transform.rotation;
            heldBody.rb.MoveRotation(heldRotation);

            if (heldBody.rb.velocity.magnitude > maxPullSpeed)
            {
                heldBody.rb.velocity = heldBody.rb.velocity.normalized * maxPullSpeed;
            }
        }
    }

    public void ApplyPull(GravityBody target)
    {
        if (isPulling)
        {
            //Debug.Log($"Attempting pull {target}");

            if (target != null && target.isGravityAffected && !target.CompareTag("Player"))
            {
                //Debug.Log($"Successful pull {target}");

                if (!bodiesInBeam.Contains(target)) bodiesInBeam.Add(target);

                Vector3 offset = transform.position - target.rb.position;
                float distance = offset.magnitude;

                if (distance <= 0)
                {
                    distance = 0.1f;
                }

                float forceMagnitude = GravityManager.Instance.gravitationalConstant * ((beamStrength * 1000) * target.rb.mass) / (distance * distance);

                Vector3 direction = offset.normalized;

                target.rb.AddForce(direction * forceMagnitude);


            }

        }
    }

    internal void LockBody(GravityBody body)
    {
        if (!bodiesInBeam.Contains(body)) return;

        if (body != null && body.isGravityAffected && !body.CompareTag("Player") && heldBody == null)
        {
            if (Time.time - body.lastReleasedTime < lockCooldown) return;


            body.rb.velocity = Vector3.zero;
            body.rb.angularVelocity = Vector3.zero;

            body.isLocked = true;
            body.rb.isKinematic = true;

            GravityManager.Instance.UnregisterBody(body);

            body.transform.SetParent(holdZone.transform);

            heldBody = body;

            bodiesInBeam.Remove(body);
            Debug.Log($"Picked up {body}");
            AudioManager.Instance.PlaySFX(OneShotSFX.Lock);

            if (body.gameObject.TryGetComponent<Cargo>(out Cargo cargo))
            {
                CarryingDisplay.Instance.UpdateCarrying(cargo);
            }
        }
    }

    public void UnlockBody(GravityBody body)
    {
        if (body != null)
        {
            body.lastReleasedTime = Time.time;

            body.isLocked = false;
            body.rb.isKinematic = false;
            body.transform.SetParent(null);

            GravityManager.Instance.RegisterBody(body);

            heldBody = null;

            AudioManager.Instance.PlaySFX(OneShotSFX.Eject);
            CarryingDisplay.Instance.ClearCarrying();
        }
    }

    public void EjectBody(GravityBody body)
    {
        Debug.Log($"Ejected {body}");

        if (body.rb == null) return;

        UnlockBody(body);
        body.rb.AddForce(transform.forward * ejectForce, ForceMode.Impulse);
    }

}