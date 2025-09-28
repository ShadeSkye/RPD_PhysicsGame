using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarkerManager : MonoBehaviour
{
    public static ObjectiveMarkerManager Instance;

    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform markerPrefab;

    [SerializeField] private float padding;

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

            if (target == null) continue;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            bool isWithinBounds = 
                screenPos.x >= padding && 
                screenPos.x <= Screen.width - padding && 
                screenPos.y >= padding && 
                screenPos.y <= Screen.height - padding;

            bool isInFront = screenPos.z > 0;
            bool isOnScreen = isInFront && isWithinBounds;

            marker.gameObject.SetActive(true);

            if (!isOnScreen)
            {
                screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
                screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);
            }

            marker.transform.position = screenPos;

            Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            marker.rotation = Quaternion.Euler(0f, 0f, angle);
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