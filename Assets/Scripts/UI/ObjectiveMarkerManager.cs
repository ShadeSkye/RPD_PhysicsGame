using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarkerManager : MonoBehaviour
{
    public static ObjectiveMarkerManager Instance;

    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform markerPrefab;

    [SerializeField] private Sprite markerSprite;
    [SerializeField] private Sprite arrowSprite;

    [SerializeField] private Sprite stationSprite;

    [SerializeField] private float padding;

    private Camera cam;

    private Dictionary<LookAtTarget, RectTransform> targetMarkers = new Dictionary<LookAtTarget, RectTransform>();

    private GameObject depotRef;

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

            UpdateMarker(target, marker);

        }

    }

    private void UpdateMarker(LookAtTarget target, RectTransform marker)
    {

        if (target == null) return;

        Image image = marker.GetComponent<Image>();
        bool isDepot = target.gameObject == depotRef;

        float angle = 0f;
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
            image.sprite = isDepot ? stationSprite : arrowSprite;

            // CLAMP
            screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
            screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

            // ROTATE
            Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float offset = isDepot ? 90f : -90f;
            angle += offset;

        }
        else
        {
            image.sprite = isDepot ? stationSprite : markerSprite;
        }

        marker.rotation = Quaternion.Euler(0f, 0f, angle);
        marker.transform.position = screenPos;
    }

    public void SetCurrentTargets(List<GameObject> targets, GameObject depot)
    {
        foreach (RectTransform m in targetMarkers.Values) Destroy(m.gameObject);

        targetMarkers.Clear();

        foreach (GameObject go in targets)
        {
            LookAtTarget target = go.GetComponentInChildren<LookAtTarget>();

            if (target == null)
                Debug.Log($"{go.name}'s LookAtTarget is null", go);

            RectTransform marker = Instantiate(markerPrefab, parent);
            targetMarkers.Add(target, marker);
        }

        depotRef = depot;

    }

}