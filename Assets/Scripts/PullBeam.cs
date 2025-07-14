using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float maxBeamDistance;
    [SerializeField, Range(0f, 1f)] private float viewWidth = 0.95f;
    [SerializeField, Range(0f, 100f)] private float beamStrength = 100f;
    [SerializeField, Range(0f, 100f)] private float lockStrength = 500f;

    private List<GravityBody> bodiesInBeam = new List<GravityBody>();

    private GravityBody heldBody;

    private void FixedUpdate()
    {
        GetObjectsAhead();
        ApplyPull();

        if (heldBody != null)
        {
            heldBody.transform.localPosition = Vector3.zero; 
        }
    }

    private void GetObjectsAhead()
    {
        bodiesInBeam.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, maxBeamDistance);
        //Debug.Log($"Overlap sphere found {hits.Length-1} objects in range");

        foreach (Collider hit in hits)
        {
            GravityBody body = hit.GetComponent<GravityBody>();

            if (body != null && body.isGravityAffected && IsInCone(body) && !bodiesInBeam.Contains(body) && !body.CompareTag("Player"))
            {
                bodiesInBeam.Add(body);
                Debug.Log($"This many: {bodiesInBeam.Count} objects in current list");
            }

        }

    }

    private void ApplyPull()
    {
        foreach (GravityBody target in bodiesInBeam)
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
            Debug.Log($"Pulling {target} by {forceMagnitude}");

        }
    }

    private bool IsInCone(GravityBody target)
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, directionToTarget);

        return (dot > viewWidth);
    }

    private void OnTriggerEnter(Collider other)
    {
        GravityBody body = other.GetComponent<GravityBody>();
        if (body != null && bodiesInBeam.Contains(body))
        {
            Debug.Log($"{body.name} entered hold zone");

            // stop moving
            body.isLocked = true;
            body.rb.velocity = Vector3.zero;
            body.rb.angularVelocity = Vector3.zero;

            body.rb.isKinematic = true;

            GravityManager.Instance.UnregisterBody(body);

            // parent to ship
            body.transform.SetParent(this.transform);

            if (heldBody == null)
            {
                heldBody = body;
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        GravityBody body = other.GetComponent<GravityBody>();
        if (body != null && !bodiesInBeam.Contains(body))
        {
            Debug.Log($"{body.name} exited hold zone");

            body.isLocked = false;
        }
    }*/

    private void HoldCargo(GravityBody target)
    {               
        Vector3 toTarget = transform.position - target.rb.position; 

        target.rb.velocity *= 0.9f; // make it slow down

        target.rb.AddForce(toTarget.normalized * lockStrength * target.rb.mass);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, maxBeamDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxBeamDistance);

        // Cone edge lines
        float coneAngle = Mathf.Acos(viewWidth) * Mathf.Rad2Deg;
        Quaternion leftRot = Quaternion.AngleAxis(-coneAngle, transform.up);
        Quaternion rightRot = Quaternion.AngleAxis(coneAngle, transform.up);

        Vector3 leftDir = leftRot * transform.forward;
        Vector3 rightDir = rightRot * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * maxBeamDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * maxBeamDistance);

    }
}
