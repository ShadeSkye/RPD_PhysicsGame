using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarkerManager : MonoBehaviour
{
    public static ObjectiveMarkerManager Instance;

    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform markerPrefab;

    private Camera cam;

    private Dictionary<LookAtTarget, RectTransform> targetMarkers = new Dictionary<LookAtTarget, RectTransform>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    void Start()
    {
        cam = Camera.main;

    }

    void Update()
    {
        foreach (KeyValuePair<LookAtTarget, RectTransform> kvp in targetMarkers)
        {
            LookAtTarget target = kvp.Key;
            RectTransform marker = kvp.Value;

            if (target == null) return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

            bool isWithinBounds = (screenPos.x >= 0 && screenPos.x <= Screen.width) && (screenPos.y >= 0 && screenPos.y <= Screen.height);
            bool isInFront = screenPos.z > 0;
            bool isOnScreen = isInFront && isWithinBounds;

            marker.gameObject.SetActive(isOnScreen);

            if (isOnScreen)
            {
                marker.transform.position = screenPos;
            }
        }

    }

    public void SetCurrentTargets(List<GameObject> targets)
    {
        foreach (RectTransform m in targetMarkers.Values) Destroy(m.gameObject);

        targetMarkers.Clear();

        foreach (GameObject go in targets)
        {
            go.TryGetComponent<LookAtTarget>(out LookAtTarget target);

            if (target == null) Debug.Log($"{go}'s look at target is null", go);

            RectTransform marker = Instantiate(markerPrefab, parent);
            targetMarkers.Add(target, marker);
        }

    }

}