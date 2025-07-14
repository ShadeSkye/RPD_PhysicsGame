using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PullBeam : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private Transform leftHolder;
    [SerializeField] private Transform rightHolder;
    [SerializeField] private float maxBeamDistance;
    [SerializeField, Range(0f, 1f)] private float viewWidth = 0.95f;

    private List<GravityBody> bodiesInBeam = new List<GravityBody>();

    private void FixedUpdate()
    {
        GetObjectsAhead();
        ApplyPull();
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
        foreach (GravityBody body in bodiesInBeam)
        {
            Debug.Log(body);
        }
    }

    private bool IsInCone(GravityBody target)
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, directionToTarget);

        return (dot > viewWidth);
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
