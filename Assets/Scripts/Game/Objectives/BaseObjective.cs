using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectiveState { InProgress, Complete, Failed }

public abstract class BaseObjective : ScriptableObject
{
    public virtual string objectiveName { get; }
    public virtual string objectiveStatus { get; }

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
    }
}