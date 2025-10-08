using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    public static ObjectiveTracker Instance;

    private TMP_Text placeholderLabel;

    [SerializeField] private RectTransform trackedObjectivelayoutGroupParent;
    [SerializeField] private TMP_Text trackedObjectiveLabelPrefab;
    public Dictionary<BaseObjective, TMP_Text> trackedObjectives = new();

    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private ObjectiveSelectButton objectiveButtonPrefab;
    private List<ObjectiveSelectButton> buttons = new List<ObjectiveSelectButton>();


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
        ResetButtons(objectives);
        ResetTracked();
    }

    public void Refresh()
    {
        RefreshButtons();
        RefreshTracked();
        UpdatePlaceholder();
    }

    private void UpdatePlaceholder()
    {
        if (trackedObjectives.Count == 0)
        {
            if (placeholderLabel == null)
            {
                placeholderLabel = Instantiate(trackedObjectiveLabelPrefab, trackedObjectivelayoutGroupParent);
                placeholderLabel.text = "Press [TAB/SELECT] to open objectives panel\nto track objectives on your HUD";
            }
            else
            {
                placeholderLabel.gameObject.SetActive(true);
            }
        }
        else
        {
            if (placeholderLabel != null)
                placeholderLabel.gameObject.SetActive(false);
        }
    }

    public void ResetTracked()
    {
        foreach (var kvp in trackedObjectives)
        {
            Destroy(kvp.Value.gameObject);
        }

        trackedObjectives.Clear();

        Refresh();
    }
    public void RefreshTracked()
    {
        foreach (var kvp in trackedObjectives)
        {
            BaseObjective objective = kvp.Key;
            TMP_Text label = kvp.Value;

            label.text = objective.GenerateLabel(true);
        }

    }

    public void ResetButtons(List<BaseObjective> objectives)
    {
        foreach (Transform child in layoutGroupParent)
        {
            Destroy(child.gameObject);
        }

        buttons.Clear();

        GameObject button = Instantiate(UIManager.Instance.ResumeButtonPrefab, layoutGroupParent);

        var sorted = SortObjectives(objectives);

        foreach (BaseObjective o in sorted)
        {
            ObjectiveSelectButton b = Instantiate(objectiveButtonPrefab, layoutGroupParent);
            b.Setup(o);
            buttons.Add(b);
        }

        Refresh();
    }

    public void RefreshButtons()
    {
        foreach (var b in buttons)
        {
            int priority;

            if (b.Objective is ExitLevelObjective)
                priority = 0;                   
            else if (b.Objective.State == ObjectiveState.Complete)
                priority = 2;                   
            else
                priority = 1;                   

            var layout = b.GetComponent<LayoutElement>();
            if (layout != null)
                layout.layoutPriority = priority;

            b.UpdateButtonState();
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroupParent);
    }


    public void ToggleTrackedObjective(BaseObjective objective)
    {
        if (trackedObjectives.ContainsKey(objective))
        {
            Destroy(trackedObjectives[objective].gameObject);
            trackedObjectives.Remove(objective);
        }
        else
        {
            TMP_Text text = Instantiate(trackedObjectiveLabelPrefab, trackedObjectivelayoutGroupParent);
            trackedObjectives[objective] = text;
        }

        HashSet<CargoType> typesToTrack = new HashSet<CargoType>();
        HashSet<CargoType> criticalTypes = new HashSet<CargoType>();

        foreach (var o in trackedObjectives.Keys)
        {
            if (o is CargoObjective co)
            {
                typesToTrack.Add(co.targetType);

                if (o.isCritical)
                {
                    criticalTypes.Add(co.targetType);
                }
            }
        }

        ObjectiveMarkerManager.Instance.SetCurrentTargetTypes(typesToTrack);
        ObjectiveMarkerManager.Instance.SetCurrentCriticalTypes(criticalTypes);

        Refresh();
    }

    private List<BaseObjective> SortObjectives(List<BaseObjective> objectives)
    {
        return objectives.OrderBy(o =>
        {
            if (o is ExitLevelObjective) return 0;

            if (o.State == ObjectiveState.InProgress || o.State == ObjectiveState.Failed) return 1;

            return 2;
        }).ToList();
    }

}
