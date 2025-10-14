using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectiveState { InProgress, Complete, Failed }

public abstract class BaseObjective : ScriptableObject
{
    public virtual string objectiveName { get; }
    public virtual string objectiveStatus { get; }

    public float objectiveValue;

    public ObjectiveState State;
    public bool isCritical;

    public abstract void ResetObjective();
    public virtual void UpdateProgress(Cargo cargo = null, float value = 0f)
    {

    }

    protected void Fail()
    {
        State = ObjectiveState.Failed;

        LevelManager.Instance.OnObjectiveFailed(this);
    }

    protected void Complete()
    {
        State = ObjectiveState.Complete;

        LevelManager.Instance.OnObjectiveComplete(this);

        CurrencyManager.Instance.AddEarnings(objectiveValue);
    }

    public string GenerateLabel(bool condense)
    {
        string text = !condense? objectiveName : $"{objectiveName}\n{objectiveStatus}";

        switch (State)
        {
            case ObjectiveState.Complete:
                text = $"<color=white><s>{text}</s></color>";
                break;
            case ObjectiveState.Failed:
                text = $"<color=red>{text}</color>";
                break;
            case ObjectiveState.InProgress:
                text = $"<color=white>{text}</color>";
                break;
        }

        if (isCritical)
        {
            text = $"<b><color=yellow>[CRITICAL] {text}</b></color>";
        }

        return text;
    }
}