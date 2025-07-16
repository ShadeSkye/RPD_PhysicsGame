using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[DefaultExecutionOrder(-1000)]

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    [Header("Gravity Settings")]
    public float gravitationalConstant = 0.001f;

    private List<GravityBody> bodies = new List<GravityBody>();
    public List<GravityBody> GetBodies() => bodies;

    [Header("Orbit Settings")]
    private bool simplifiedSimulation = true; // if its off its like nbody if its on its just the strongest
    [SerializeField, Range(0f, 100f)] private float alignedOrbitPercentage = 60f;
    public float alignedOrbitChance => alignedOrbitPercentage / 100f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterBody(GravityBody body)
    {
        if (!bodies.Contains(body))
            bodies.Add(body);
    }

    public void UnregisterBody(GravityBody body)
    {
        bodies.Remove(body);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        for (int x = 0; x < bodies.Count; x++) // for each body
        {
            GravityBody a = bodies[x];

            if (!a.isGravityAffected) continue; // skip if it is planet

            Vector3 totalForce = Vector3.zero;

            if (!simplifiedSimulation)
            {
                for (int y = 0; y < bodies.Count; y++) // for each body that affects it
                {
                    if (x != y)
                    {
                        GravityBody b = bodies[y];

                        totalForce += CalculateGravity(a, b);

                    }

                }
            }
            else
            {
                GravityBody strongest = GetStrongestGravitySource(a);
                if (strongest != null)
                {
                    if (a.CompareTag("Player") && strongest.dontPullPlayer)
                    {
                        //Debug.Log($"Skipping gravity from {strongest.name} to Player due to dontPullPlayer");
                        continue;
                    }

                    totalForce += CalculateGravity(a, strongest);
                }
            }

            a.rb.AddForce(totalForce);
        }
    }

    private Vector3 CalculateGravity(GravityBody a, GravityBody b)
    {
        Vector3 offset = b.rb.position - a.rb.position;
        float distance = offset.magnitude;

        if (distance <= 0)
        {
            distance = 0.1f;
        }

        float forceMagnitude = gravitationalConstant * (a.rb.mass * b.rb.mass) / (distance * distance);

        Vector3 direction = offset.normalized;

        return direction * forceMagnitude;
    }

    private GravityBody GetStrongestGravitySource(GravityBody a)
    {
        GravityBody strongestBody = null;
        float strongestForce = 0;

        foreach (GravityBody body in bodies)
        {
            if (body == a || !body.rb.isKinematic) continue;

            float distance = Vector3.Distance(a.rb.position, body.rb.position);

            if (distance <= 0)
            {
                distance = 0.1f;
            }

            float force = body.rb.mass / (distance * distance);

            if (force > strongestForce)
            {
                strongestForce = force;
                strongestBody = body;
            }
        }

        return strongestBody;
    }
}