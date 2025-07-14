using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float beamStrength = 10000;
    [SerializeField] private float maxPullSpeed = 10000;

    [Header("Zone Settings")]
    [SerializeField] private Collider pullZone;
    [SerializeField] private Collider holdZone;

    [Header("Hold Settings")]
    private GravityBody heldBody;
    public GravityBody HeldBody => heldBody;
    [SerializeField] private float radiusPadding = 1f;
    [SerializeField] private float ejectForce;
    private Vector3 targetWorldPos;

    public bool isPulling;
    private float lockCooldown = 1f;

    private void FixedUpdate()
    {
        if (heldBody != null)
        {
            Vector3 targetWorldPos = holdZone.transform.position + holdZone.transform.forward * (heldBody.radius + radiusPadding);
            heldBody.rb.MovePosition(targetWorldPos);

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
            if (target != null && target.isGravityAffected && !target.CompareTag("Player"))
            {
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

        if (body != null && body.isGravityAffected && !body.CompareTag("Player") && heldBody == null)
        {
            if (Time.time - body.lastReleasedTime < lockCooldown) return;

            body.isLocked = true;
            body.rb.velocity = Vector3.zero;
            body.rb.angularVelocity = Vector3.zero;

            body.rb.isKinematic = true;

            GravityManager.Instance.UnregisterBody(body);

            body.transform.SetParent(holdZone.transform);

            heldBody = body;

            Debug.Log($"Picked up {body}");
        }
    }

    public void UnlockBody(GravityBody body)
    {
        if (body != null)
        {
            body.lastReleasedTime = Time.time;

            body.isLocked = false;
            body.rb.isKinematic = false;

            GravityManager.Instance.RegisterBody(body);

            body.transform.SetParent(null);

            heldBody = null;
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
