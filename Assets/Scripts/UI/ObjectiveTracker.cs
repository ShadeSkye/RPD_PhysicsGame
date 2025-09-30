using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public static ObjectiveTracker Instance;

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
}
