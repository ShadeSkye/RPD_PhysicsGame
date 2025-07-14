using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class OrbitGizmoDrawer : MonoBehaviour
{
    [Header("Orbit Preview Settings")]
    public Color orbitColor = Color.yellow;
    public Color velocityColor = Color.cyan;
    [Range(10, 1000)] public int steps = 300;
    public float timeStep = 0.02f;
    public bool drawOrbits = true;

    private void OnDrawGizmos()
    {
        if (!drawOrbits || GravityManager.Instance == null) return;

        var bodies = GravityManager.Instance.GetBodies();

        foreach (var body in bodies)
        {
            if (body == null || body.rb == null || body.rb.isKinematic)
                continue;

            Vector3[] orbitPath = SimulateOrbit(body, bodies);
            Gizmos.color = orbitColor;

            for (int i = 1; i < orbitPath.Length; i++)
            {
                Gizmos.DrawLine(orbitPath[i - 1], orbitPath[i]);
            }

            // Optional: draw velocity vector
            Gizmos.color = velocityColor;
            Gizmos.DrawLine(body.rb.position, body.rb.position + body.rb.velocity);
        }
    }

    private Vector3[] SimulateOrbit(GravityBody target, List<GravityBody> allBodies)
    {
        List<Vector3> path = new List<Vector3>();

        // Start from current position and velocity
        Vector3 simulatedPos = target.rb.position;
        Vector3 simulatedVel = target.rb.velocity;

        for (int step = 0; step < steps; step++)
        {
            Vector3 acceleration = Vector3.zero;

            foreach (var other in allBodies)
            {
                if (other == target || other == null || other.rb == null || !other.rb.isKinematic)
                    continue;

                Vector3 offset = other.rb.position - simulatedPos;
                float distance = Mathf.Max(offset.magnitude, 0.1f);
                Vector3 direction = offset.normalized;

                float accMagnitude = GravityManager.Instance.gravitationalConstant *
                                     other.rb.mass / (distance * distance);

                acceleration += direction * accMagnitude;
            }

            // Update velocity and position
            simulatedVel += acceleration * timeStep;
            simulatedPos += simulatedVel * timeStep;

            path.Add(simulatedPos);
        }

        return path.ToArray();
    }
}
