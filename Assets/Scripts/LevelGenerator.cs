using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [SerializeField] private GameObject spaceship;
    private Rigidbody rb;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject cargoPrefab;

    public List<Transform> Planets = new List<Transform>();
    public List<float> PlanetSizes = new List<float>();

    [Header("Settings")]
    [SerializeField] private int planetCount;
    [SerializeField] private int cargoCount;
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
        for (int i = 0; i < planetCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnArea / 2f, spawnArea / 2f), Random.Range(-spawnArea / 2f, spawnArea / 2f), Random.Range(-spawnArea / 2f, spawnArea / 2f));
            GameObject newAsteroid = Instantiate(planetPrefab, spawnPos, Random.rotation);

            float scale = Random.Range(sizeRange.x, sizeRange.y);
            newAsteroid.transform.localScale = Vector3.one * scale;

            Planets.Add(newAsteroid.transform);
            PlanetSizes.Add(scale);   
        }

        for (int i = 0; i < cargoCount; i++)
        {
            // spawn on planet
            Transform randomPlanet = Planets[Random.Range(0, Planets.Count)];

            // instantiate
            GameObject newCargo = Instantiate(cargoPrefab, randomPlanet.transform.position, Quaternion.identity);
            GravityBody body = newCargo.GetComponent<GravityBody>();
            Cargo cargo = newCargo.GetComponent<Cargo>();

            // set up body
            body.transform.localScale = Vector3.one * Random.Range(0.3f, 0.5f);
            body.orbitDistance = Random.Range(5f, 15f);
            body.orbitTarget = randomPlanet.GetComponent<GravityBody>();

            cargo.cargoName = $"Crate {i}";
            cargo.weight = Random.Range(5f, 15f);
            cargo.baseValue = Random.Range(50f, 150f);
        }
    }
}
