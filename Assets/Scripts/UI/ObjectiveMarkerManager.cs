using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public enum MarkerSprites
{
    marker,
    sideMarker,
    arrow,
    spaceStation
}

public class ObjectiveMarkerManager : MonoBehaviour
{
    public static ObjectiveMarkerManager Instance;

    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform markerPrefab;

    [SerializeField] private Sprite[] markerSprites;

    [SerializeField] private float padding;

    [SerializeField] int maxPerType = 10;

    [Header("Scale")]
    [SerializeField] float defaultScale = 1f;
    [SerializeField] float minScale = 0.5f;
    [SerializeField] float closeDistance = 100f;
    [SerializeField] float maxDistance = 5000f;

    private Dictionary<LookAtTarget, RectTransform> targetMarkers = new Dictionary<LookAtTarget, RectTransform>();

    private GameObject depotRef;

    private HashSet<CargoType> targetTypes = new HashSet<CargoType>();
    private HashSet<CargoType> criticalTargetTypes = new HashSet<CargoType>();

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

        Cargo cargo = target.gameObject.GetComponent<Cargo>();
        if (cargo != null) // if it is cargo
        {
            if (!targetTypes.Contains(cargo.type))
            {
                marker.gameObject.SetActive(false);
                return;
            }
        }

        bool isWithinBounds;

        Image image = marker.GetComponent<Image>();
        bool isDepot = target.gameObject == depotRef;

        float angle = 0f;
        Vector3 screenPos = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector3 toTarget = target.transform.position - Camera.main.transform.position;
        float distance = toTarget.magnitude;

        bool isInFront = Vector3.Dot(Camera.main.transform.forward, toTarget) > 0f;

        marker.gameObject.SetActive(true);

        if (isInFront)
        {
            screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

            isWithinBounds =
            screenPos.x >= padding &&
            screenPos.x <= Screen.width - padding &&
            screenPos.y >= padding &&
            screenPos.y <= Screen.height - padding;

            if (!isWithinBounds)
            {

                // CLAMP
                screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
                screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

                // ROTATE
                Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                angle += isDepot ? 90f : -90f;

            }
        }
        else // if behind player
        {
            isWithinBounds = false;

            Vector3 camRight = Camera.main.transform.right;
            Vector3 camUp = Camera.main.transform.up;

            Vector2 dir2D = new Vector2(Vector3.Dot(toTarget, camRight), Vector3.Dot(toTarget, camUp));
            if (dir2D.sqrMagnitude < 0.001f) dir2D = Vector2.up; 
            dir2D.Normalize();

            float radiusX = Screen.width / 2f - padding;
            float radiusY = Screen.height / 2f - padding;
            screenPos = new Vector3(
                screenCenter.x + dir2D.x * radiusX,
                screenCenter.y + dir2D.y * radiusY,
                0f
            );

            angle = Mathf.Atan2(dir2D.y, dir2D.x) * Mathf.Rad2Deg;
            angle += isDepot ? 90f : -90f;
        }


        // rotation
        marker.rotation = Quaternion.Euler(0f, 0f, angle);
        // position
        screenPos.z = 0f;
        marker.transform.position = screenPos;
        // scale
        float farScale = Mathf.Lerp(defaultScale, minScale, Mathf.Clamp01(distance / maxDistance));
        float closeScale = distance < closeDistance
            ? Mathf.Lerp(minScale, defaultScale, distance / closeDistance)
            : defaultScale;
        float finalScale = Mathf.Min(farScale, closeScale);
        marker.localScale = new Vector3(finalScale, finalScale, 1f);

        // sprite
        SetSprite(target, image, isWithinBounds, isDepot);
    }

    private void SetSprite(LookAtTarget target, Image image, bool isOnScreen, bool isDepot)
    {
        // default
        MarkerSprites sprite = MarkerSprites.sideMarker;
        Color color = Color.white;

        if(target.gameObject.GetComponent<Cargo>() != null)
        {
            CargoType type = target.gameObject.GetComponent<Cargo>().type;

            if (!isOnScreen)
            {
                sprite = MarkerSprites.arrow;
            }
            else if (criticalTargetTypes.Contains(type))
            {
                sprite = MarkerSprites.marker;
            }
            else
            {
                sprite = MarkerSprites.sideMarker;
            }


            // set color
            if (criticalTargetTypes.Contains(type))
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.blue;
            }

        }
        else
        {

            if (isDepot)
            {
                sprite = MarkerSprites.spaceStation;
                color = Color.white;
            }
        }

        image.sprite = markerSprites[(int)sprite];
        image.color = color;

    }

    public void SetCurrentTargetTypes(HashSet<CargoType> types)
    {
        targetTypes = types;
    }

    public void SetCurrentCriticalTypes(HashSet<CargoType> types)
    {
        criticalTargetTypes = types;
    }

    public void SetCurrentTargets(List<GameObject> targets, GameObject depot)
    {
        foreach (RectTransform m in targetMarkers.Values) Destroy(m.gameObject);
        targetMarkers.Clear();

        Dictionary<CargoType, int> typeCounts = new Dictionary<CargoType, int>();

        if (depot != null)
        {
            LookAtTarget depotTarget = depot.GetComponentInChildren<LookAtTarget>();
            if (depotTarget != null)
            {
                RectTransform depotMarker = Instantiate(markerPrefab, parent);
                targetMarkers.Add(depotTarget, depotMarker);
            }
        }

        targets.Sort((a, b) =>
        {
            float distA = (a.transform.position - Camera.main.transform.position).sqrMagnitude;
            float distB = (b.transform.position - Camera.main.transform.position).sqrMagnitude;
            return distA.CompareTo(distB);
        });

        foreach (GameObject go in targets)
        {
            if (go == depot) continue;

            Cargo cargo = go.GetComponent<Cargo>();
            CargoType type = cargo != null ? cargo.type : default;

            if (!typeCounts.ContainsKey(type))
                typeCounts[type] = 0;

            if (typeCounts[type] >= maxPerType)
                continue; 

            LookAtTarget target = go.GetComponentInChildren<LookAtTarget>();
            if (target == null)
            {
                Debug.Log($"{go.name}'s LookAtTarget is null", go);
                continue;
            }

            RectTransform marker = Instantiate(markerPrefab, parent);
            targetMarkers.Add(target, marker);

            typeCounts[type]++;
        }

        depotRef = depot;
    }

}