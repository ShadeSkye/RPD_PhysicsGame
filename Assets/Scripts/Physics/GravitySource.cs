using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : GravityObject
{
    public bool DontPullPlayer = false;

    [SerializeField] private float localGravity = 1f;
    public float Radius = 1f;

    protected override void Awake()
    {
        base.Awake();
        rb.isKinematic = true;

        localGravity = localGravity == 0 ? 1f : localGravity;
        Radius = Radius == 0 ? 1f : Radius;
        rb.mass = (localGravity * Radius * Radius) / GravityManager.Instance.gravitationalConstant;
        transform.localScale = Vector3.one * Radius * 2;
    }

    private void OnValidate()
    {
        UpdateSize();
    }

    private void UpdateSize()
    {
        // set size using radius
        Radius = Radius == 0 ? 1f : Radius;
        transform.localScale = Vector3.one * Radius * 2;
    }
}
