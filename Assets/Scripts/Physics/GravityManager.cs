using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-1000)]
public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance { get; private set; }

    [Header("Gravity Settings")]
    public float gravitationalConstant = 0.001f;

    private List<GravityBody> bodies = new List<GravityBody>();
    public List<GravityBody> GetBodies() => bodies;

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
        // Debug.Log("GravityManager running");
        ApplyGravity();

        /*foreach (var body in bodies)
        {
            if (!body.rb.isKinematic)
                body.UpdatePosition();

        }*/
    }

    private void ApplyGravity()
    {
        for (int x = 0; x < bodies.Count; x++) // for each body
        {
            GravityBody a = bodies[x];

            if (a.rb.isKinematic) continue; // skip if it is planet

            Vector3 totalForce = Vector3.zero;

            for (int y = 0; y < bodies.Count; y++) // for each body that affects it
            {
                if(x != y)
                {

                    GravityBody b = bodies[y];

                    Vector3 offset = b.rb.position - a.rb.position;
                    float distance = offset.magnitude;
                    Vector3 direction = offset.normalized;

                    if(distance <= 0) 
                    {
                        distance = 0.1f;
                    }

                    float forceMagnitude = gravitationalConstant * (a.rb.mass * b.rb.mass) / (distance * distance);
                    totalForce += direction * forceMagnitude;

                    /*float accelerationMagnitude = gravitationalConstant * b.rb.mass / (distance * distance);
                    totalForce += direction * accelerationMagnitude; */

                }

            }

            a.rb.AddForce(totalForce);
            //a.UpdateVelocity(acceleration);
        }
    }
}
