using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [HideInInspector] public string displayName = "Unknown Object";
    public float distanceToPlayer
    {
        get
        {
            if (PlayerManager.Instance != null)
                return Vector3.Distance(PlayerManager.Instance.transform.position, transform.position);
            else
                return 0f;
        }
    }

}
