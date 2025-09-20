using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class GravityGizmos : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw orbits for all bodies in the scene
        GravityBody[] bodies = FindObjectsOfType<GravityBody>();

        foreach (GravityBody body in bodies)
        {
            if (body.OrbitTarget == null) continue;

            Vector3 center = body.OrbitTarget.transform.position;
            float radius = body.OrbitTarget.Radius + body.OrbitDistance;

            // Choose color based on alignment chance
            Gizmos.color = Color.Lerp(Color.yellow, Color.cyan, UnityEngine.Random.value);

            // Draw a simple wire disc in the XY plane
            DrawOrbit(body.transform, center, radius);
        }
    }

    private void DrawOrbit(Transform bodyTransform, Vector3 center, float radius)
    {
        const int segments = 64;
        Vector3 lastPoint = center + Vector3.right * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }
#endif
}
