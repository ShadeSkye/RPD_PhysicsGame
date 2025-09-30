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
        string text = $"{myObjective.objectiveName}\n{myObjective.objectiveStatus}";

        switch (myObjective.State)
        {
            case ObjectiveState.Complete:
                buttonText.color = Color.white;
                text = $"<s>{text}</s>";
                break;
            case ObjectiveState.Failed:
                buttonText.color = Color.red;
                break;
            case ObjectiveState.InProgress:
                buttonText.color = Color.white;
                break;
        }

        if (myObjective.isCritical)
        {
            text = $"<b>{text}</b>";
        }

        buttonText.text = text;

    }

    private void OnButtonClicked()
    {
        

    }
}
