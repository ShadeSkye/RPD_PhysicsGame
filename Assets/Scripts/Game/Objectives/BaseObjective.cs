using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    public string objectiveName;
    [HideInInspector] public bool isComplete;
    [HideInInspector] public bool isFailed;
    public bool isCritical;

    public abstract void ResetObjective();
    public virtual void AddProgress(Cargo cargo = null, float value = 0f)
    {
        if (isComplete || isFailed) return;
    }

    protected void Fail()
    {
        isComplete = false;
        isFailed = true;

        LevelManager.Instance.OnObjectiveFailed(this);
    }

    protected void Complete()
    {
        isComplete = true;
        isFailed = false;

        LevelManager.Instance.OnObjectiveComplete(this);
    }
}