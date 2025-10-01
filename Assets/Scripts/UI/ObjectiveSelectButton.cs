using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveSelectButton : MonoBehaviour
{
    private BaseObjective myObjective;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Button button;

    private bool isTracked => ObjectiveTracker.Instance.trackedObjectives.ContainsKey(myObjective);

    internal void Setup(BaseObjective o)
    {
        myObjective = o;

        button.onClick.AddListener(() =>
        {
            OnButtonClicked();
        });

        UpdateButtonState();
    }

    public void UpdateButtonState()
    {
        buttonText.text = myObjective.GenerateLabel(false);

        buttonImage.color = isTracked ? Color.blue : Color.white;

    }

    private void OnButtonClicked()
    {
        ObjectiveTracker.Instance.ToggleTrackedObjective(myObjective);

    }
}
