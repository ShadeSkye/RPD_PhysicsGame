using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

        RefreshTracked();
        UpdatePlaceholder();
    }
    public void RefreshTracked()
    {
        foreach (var kvp in trackedObjectives)
        {
            BaseObjective objective = kvp.Key;
            TMP_Text label = kvp.Value;

            label.text = objective.GenerateLabel();
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

        foreach (BaseObjective o in objectives)
        {
            ObjectiveSelectButton b = Instantiate(objectiveButtonPrefab, layoutGroupParent);
            b.Setup(o);
            buttons.Add(b);
        }

        RefreshButtons();
    }

    public void RefreshButtons()
    {
        foreach (var b in buttons)
        {
            b.UpdateButtonState();
        }
        
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
        foreach (var o in trackedObjectives.Keys)
        {
            if (o is CargoObjective co)
                typesToTrack.Add(co.targetType);
        }

        ObjectiveMarkerManager.Instance.SetCurrentTargetTypes(typesToTrack);

        RefreshButtons();
        RefreshTracked();
        UpdatePlaceholder();
    }

}
