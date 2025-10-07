using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LookAtDisplay : MonoBehaviour
{
    public static LookAtDisplay Instance { get; private set; }

    [SerializeField] TextMeshProUGUI ObjectName;
    [SerializeField] TextMeshProUGUI Distance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int mask = ~(1 << 2);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Hit: " + hit.collider.name);

            LookAtTarget target = FindInParents(hit.collider.transform);
            if (target != null)
            {
                UpdateLookAtObject(target.displayName, target.distanceToPlayer);
            }
            else
            {
                ClearDisplay();
            }

        }
        else
        {
            ClearDisplay();
        }

        //Debug.Log(dmg.damagePercent);
    }

    LookAtTarget FindInParents(Transform t)
    {
        while (t != null)
        {
            if (t.TryGetComponent<LookAtTarget>(out LookAtTarget target))
                return target;
            t = t.parent;
        }
        return null;
    }

    private void UpdateName(string name)
    {
        if (string.IsNullOrEmpty(name)) name = "Unknown Object";
        ObjectName.text = name;
    }
     private void UpdateDistance(float dist)
     {
        string formatted = dist.ToString("F0");
        Distance.text = $"Distance: {formatted}m";
     }

    public void UpdateLookAtObject(string name, float dist)
     {
        UpdateName(name);
        UpdateDistance(dist);
     }

    public void ClearDisplay()
    {
        ObjectName.text = "";
        Distance.text = "";
    }
}
