using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public static ObjectiveTracker Instance;

    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private TMP_Text objectiveTextPrefab;
    
    private Dictionary<BaseObjective, TMP_Text> objectiveLabels = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(List<BaseObjective> objectives)
    {
        foreach (Transform child in layoutGroupParent)
        {
            Destroy(child.gameObject);
        }

        objectiveLabels.Clear();

        foreach (BaseObjective o in objectives)
        {
            TMP_Text text = Instantiate(objectiveTextPrefab, layoutGroupParent);
            objectiveLabels[o] = text;
        }

        UpdateText();
    }

    public void UpdateText()
    {
        foreach (var kvp in objectiveLabels)
        {
            BaseObjective objective = kvp.Key;
            TMP_Text label = kvp.Value;

            string prefix = objective.isCritical ? "[C] " : "";

            label.text = $"{prefix}{objective.objectiveName}: {objective.objectiveStatus}";
        }
    }
}
