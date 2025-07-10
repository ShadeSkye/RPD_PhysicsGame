using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public static AsteroidManager Instance;

    [SerializeField] private GameObject spaceship;
    private Rigidbody rb;
    [SerializeField] private GameObject asteroidPrefab;

    public List<Transform> Asteroids = new List<Transform>();
    public List<float> AsteroidSizes = new List<float>();

    [Header("Settings")]
    [SerializeField] private int asteroidCount;
    [SerializeField] private float spawnArea;
    [SerializeField] private Vector2 sizeRange;
    [SerializeField] private float gravityRadius;
    [SerializeField] private float gravityStrength;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        rb = spaceship.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigidbody for spaceship is null");
        }
    }

    void Start()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnArea / 2f, spawnArea / 2f), Random.Range(-spawnArea / 2f, spawnArea / 2f), Random.Range(-spawnArea / 2f, spawnArea / 2f));
            GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPos, Random.rotation);

            float scale = Random.Range(sizeRange.x, sizeRange.y);
            newAsteroid.transform.localScale = Vector3.one * scale;

            Asteroids.Add(newAsteroid.transform);
            AsteroidSizes.Add(scale);   
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform asteroid in Asteroids)
        {
            float size = asteroid.localScale.x;
            float pullRadius = size * gravityRadius;
            float pullStrength = size * gravityStrength;

            float distance = Vector3.Distance(asteroid.position, spaceship.transform.position); 
            if (distance < pullRadius)
            {
                Vector3 direction = (asteroid.position - spaceship.transform.position).normalized;
                Vector3 force = direction * pullStrength;
                rb.AddForceAtPosition(force, asteroid.transform.position);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        foreach (Transform asteroid in Asteroids)
        {
            float size = asteroid.localScale.x;
            float pullRadius = size * gravityRadius;

            Gizmos.DrawWireSphere(asteroid.position, pullRadius);
        }
    }
}
