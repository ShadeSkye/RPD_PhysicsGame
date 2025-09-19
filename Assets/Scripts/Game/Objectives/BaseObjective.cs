using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    public string objectiveName;
    [HideInInspector] public bool isComplete;

    public abstract void ResetObjective();
    public abstract void AddProgress(Cargo cargo = null, float value = 0f);
}