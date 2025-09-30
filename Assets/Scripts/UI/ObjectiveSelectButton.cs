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
        buttonText.text = myObjective.GenerateLabel();

    }

    private void OnButtonClicked()
    {
        ObjectiveTracker.Instance.UpdateTrackedObjective(myObjective);
        UIManager.Instance.ResumeGame();

    }
}
