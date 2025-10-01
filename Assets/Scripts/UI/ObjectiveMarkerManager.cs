using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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

    private Dictionary<LookAtTarget, RectTransform> targetMarkers = new Dictionary<LookAtTarget, RectTransform>();

    private GameObject depotRef;

    private HashSet<CargoType> targetTypes = new HashSet<CargoType>();

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

        Image image = marker.GetComponent<Image>();
        bool isDepot = target.gameObject == depotRef;

        float angle = 0f;
        Vector3 screenPos = Vector3.zero;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector3 toTarget = target.transform.position - Camera.main.transform.position;
        bool isInFront = Vector3.Dot(Camera.main.transform.forward, toTarget) > 0f;

        marker.gameObject.SetActive(true);

        if (isInFront)
        {
            screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

            bool isWithinBounds =
            screenPos.x >= padding &&
            screenPos.x <= Screen.width - padding &&
            screenPos.y >= padding &&
            screenPos.y <= Screen.height - padding;

            if (!isWithinBounds)
            {
                image.sprite = isDepot ? stationSprite : arrowSprite;

                // CLAMP
                screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
                screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

                // ROTATE
                Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                angle += isDepot ? 90f : -90f;

            }
            else
            {
                image.sprite = isDepot ? stationSprite : markerSprite;
            }
        }
        else // if behind player
        {
            image.sprite = isDepot ? stationSprite : arrowSprite;

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


        marker.rotation = Quaternion.Euler(0f, 0f, angle);

        screenPos.z = 0f;

        marker.transform.position = screenPos;


    }

    public void SetCurrentTargetTypes(HashSet<CargoType> types)
    {
        targetTypes = types;
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