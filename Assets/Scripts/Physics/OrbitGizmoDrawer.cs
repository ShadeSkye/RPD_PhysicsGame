using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class OrbitGizmoDrawer : MonoBehaviour
{
    [Header("Orbit Preview Settings")]
    public Color orbitColor = Color.yellow;
    public Color velocityColor = Color.cyan;
    [Range(10, 1000)] public int steps = 300;
    public float timeStep = 0.02f;
    public bool drawOrbits = true;
    public bool drawInGame = true;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = orbitColor;
        lr.endColor = orbitColor;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.loop = false;
        lr.enabled = drawInGame;
    }

    private void OnDrawGizmos()
    {
        if (!drawOrbits || GravityManager.Instance == null) return;

        var bodies = GravityManager.Instance.GetBodies();
        var sources = GravityManager.Instance.GetSources();

        foreach (var body in bodies)
        {
            if (body == null || body.rb == null || (body is Cargo cargo && cargo.IsLocked))
                continue;

            Vector3[] orbitPath = SimulateOrbit(body, sources);

            // Draw Gizmos in Scene view
            Gizmos.color = orbitColor;
            for (int i = 1; i < orbitPath.Length; i++)
                Gizmos.DrawLine(orbitPath[i - 1], orbitPath[i]);

            // Optional: draw velocity vector
            Gizmos.color = velocityColor;
            Gizmos.DrawLine(body.rb.position, body.rb.position + body.rb.velocity);

            // Draw in Game view with LineRenderer
            if (drawInGame && lr != null)
            {
                lr.positionCount = orbitPath.Length;
                lr.SetPositions(orbitPath);
            }
        }
    }

    private Vector3[] SimulateOrbit(GravityBody target, List<GravitySource> sources)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 simulatedPos = target.rb.position;
        Vector3 simulatedVel = target.rb.velocity;

        for (int step = 0; step < steps; step++)
        {
            Vector3 acceleration = Vector3.zero;

            foreach (var source in sources)
            {
                if (source == null || source.rb == null) continue;

                Vector3 offset = source.rb.position - simulatedPos;
                float distance = Mathf.Max(offset.magnitude, 0.1f);
                Vector3 direction = offset.normalized;

                float accMagnitude = GravityManager.Instance.gravitationalConstant *
                                     source.rb.mass / (distance * distance);

                acceleration += direction * accMagnitude;
            }

            simulatedVel += acceleration * timeStep;
            simulatedPos += simulatedVel * timeStep;

            path.Add(simulatedPos);
        }

        return path.ToArray();
    }
}
