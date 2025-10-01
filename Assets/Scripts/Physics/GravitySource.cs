using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : GravityObject
{
    [Header("Gravity Settings")]
    public bool DontPullPlayer = false;

    [SerializeField] private float gravityStrength = 10f;
    private float localGravity => Mathf.Pow(10, gravityStrength);
    //[HideInInspector] 
    public float Radius = 1f;

    [Header("Rotation Settings")]
    public bool rotate = true;
    public float rotationSpeed = 5f;

    private Vector3 rotationAxis;

    protected void Reset()
    {
        isDynamic = false;
    }
    protected override void Awake()
    {
        base.Awake();
        UpdateRadius();

        rb.mass = (localGravity * Radius * Radius) / GravityManager.Instance.gravitationalConstant;

        GravityManager.Instance.RegisterObject(this);

        rotationAxis = Random.onUnitSphere;
    }

    private void Update()
    {
        if (rotate) transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnValidate()
    {
        UpdateRadius();
    }

    private void UpdateRadius()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            Vector3 worldSize = mr.bounds.size; 
            Radius = Mathf.Max(worldSize.x, worldSize.y, worldSize.z) * 0.5f;
        }
        else
        {
            Radius = transform.localScale.x * 0.5f;
        }
    }
}
