using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(LookAtTarget))]
public class GravityObject : MonoBehaviour
{
    public Rigidbody rb;
    public string objectName;

    protected LookAtTarget lookAt;

    [SerializeField] protected bool isDynamic;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = !isDynamic;
        
        if (lookAt == null) lookAt = GetComponent<LookAtTarget>();
        lookAt.displayName = objectName;
    }
}
