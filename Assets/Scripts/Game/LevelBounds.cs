using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public static LevelBounds Instance;

    [Header("Bounds Settings")]
    [SerializeField] private Vector3 boundsSize = new Vector3(100f, 100f, 100f);
    public Vector3 SpawnArea => boundsSize * 0.75f;

    public float wallThickness = 5f;
    private BoxCollider[] colliders;

    private Transform player;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        GenerateBoundsColliders();

    }

    void OnValidate()
    {
        GenerateBoundsColliders();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (!player)
            Debug.LogError("Player not found for LevelBounds slowdown system.");
    }
    private void GenerateBoundsColliders()
    {
        colliders = GetComponentsInChildren<BoxCollider>();

        Vector3 half = boundsSize * 0.5f;

        // Left / Right
        colliders[1].transform.localPosition = new Vector3(-half.x - wallThickness * 0.5f, 0, 0);
        colliders[2].transform.localPosition = new Vector3(half.x + wallThickness * 0.5f, 0, 0);

        // Top / Bottom
        colliders[3].transform.localPosition = new Vector3(0, half.y + wallThickness * 0.5f, 0);
        colliders[4].transform.localPosition = new Vector3(0, -half.y - wallThickness * 0.5f, 0);

        // Front / Back
        colliders[5].transform.localPosition = new Vector3(0, 0, half.z + wallThickness * 0.5f);
        colliders[6].transform.localPosition = new Vector3(0, 0, -half.z - wallThickness * 0.5f);

        Vector3 wallSizeX = new Vector3(wallThickness, boundsSize.y, boundsSize.z);
        Vector3 wallSizeY = new Vector3(boundsSize.x, wallThickness, boundsSize.z);
        Vector3 wallSizeZ = new Vector3(boundsSize.x, boundsSize.y, wallThickness);

        colliders[1].size = wallSizeX;
        colliders[2].size = wallSizeX;
        colliders[3].size = wallSizeY;
        colliders[4].size = wallSizeY;
        colliders[5].size = wallSizeZ;
        colliders[6].size = wallSizeZ;
    }

    public void Teleport(GameObject target)
    {
        Vector3 randomLocation = new Vector3(
           Random.Range(-SpawnArea.x, SpawnArea.x),
           Random.Range(-SpawnArea.y, SpawnArea.y),
           Random.Range(-SpawnArea.z, SpawnArea.z)
       );

        //Debug.Log($"Teleporting {target}to {randomLocation}!");

        target.transform.position = randomLocation;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
