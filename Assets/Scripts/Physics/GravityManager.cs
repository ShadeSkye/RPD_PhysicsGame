using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[DefaultExecutionOrder(-1000)]

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    [Header("Gravity Settings")]
    public float gravitationalConstant = 0.001f;
    [SerializeField] private bool simplifiedSimulation = false;

    [Header("Orbit Settings")]
    [SerializeField, Range(0f, 100f)] private float alignedOrbitPercentage = 60f;
    public float alignedOrbitChance => alignedOrbitPercentage / 100f;

    private List<GravityBody> bodies = new List<GravityBody>();
    private List<GravitySource> sources = new List<GravitySource>();
    public List<GravityBody> GetBodies() => bodies;
    public List<GravitySource> GetSources() => sources;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterObject(GravityObject obj)
    {
        switch (obj)
        {
            case GravityBody body when !bodies.Contains(body):
                bodies.Add(body);
                break;
            case GravitySource source when !sources.Contains(source):
                sources.Add(source);
                break;
        }
    }

    public void UnregisterObject(GravityObject obj)
    {
        switch (obj)
        {
            case GravityBody body:
                bodies.Remove(body);
                break;
            case GravitySource source:
                sources.Remove(source);
                break;
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        foreach (var body in bodies)
        {
            if (body is Cargo cargo && cargo.IsLocked) continue;

            Vector3 totalForce = Vector3.zero;

            if (!simplifiedSimulation)
            {
                foreach (var source in sources)
                {
                    if (source == body) continue;
                    if (body.CompareTag("Player") && source.DontPullPlayer) continue;

                    totalForce += CalculateGravity(body, source);
                }
            }
            else
            {
                GravitySource strongest = GetStrongestSource(body);
                if (strongest != null && !(body.CompareTag("Player") && strongest.DontPullPlayer))
                {
                    totalForce += CalculateGravity(body, strongest);
                }
            }

            if(body is Cargo c && c.type == CargoType.Egg) continue;

            body.rb.AddForce(totalForce);
        }
    }

    private Vector3 CalculateGravity(GravityBody body, GravitySource source)
    {
        Vector3 offset = source.rb.position - body.rb.position;
        float distance = Mathf.Max(offset.magnitude, 0.1f);
        float forceMagnitude = gravitationalConstant * (body.rb.mass * source.rb.mass) / (distance * distance);
        return offset.normalized * forceMagnitude;
    }

    private GravitySource GetStrongestSource(GravityBody body)
    {
        GravitySource strongest = null;
        float maxForce = 0f;

        foreach (var source in sources)
        {
            if (source == body) continue;

            float distance = Vector3.Distance(body.rb.position, source.rb.position);
            distance = Mathf.Max(distance, 0.1f);

            float force = source.rb.mass / (distance * distance);
            if (force > maxForce)
            {
                maxForce = force;
                strongest = source;
            }
        }

        return strongest;
    }
}