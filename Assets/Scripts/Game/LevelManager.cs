using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public LevelData LevelData;
    public List<Cargo> deliveredCargo = new List<Cargo>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        foreach (var obj in LevelData.objectives) obj.ResetObjective();
    }

    public void OnCargoDelivered(Cargo c)
    {
        deliveredCargo.Add(c);

        foreach (var obj in LevelData.objectives)
            obj.AddProgress(c);

        CheckLevelComplete();
    }

    public void OnObjectiveComplete(BaseObjective o)
    {
        Debug.Log($"Objective {o.objectiveName} complete");
        CheckLevelComplete();
    }

    public void OnObjectiveFailed(BaseObjective o)
    {
        Debug.Log($"Objective {o.objectiveName} failed {(o.isCritical ? "(critical)" : "(optional)")}");
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (LevelData.objectives.Any(o => o.isCritical && o.isFailed))
        {
            OnLevelFail();
            return;
        }

        if (LevelData.objectives
            .Where(o => o.isCritical)
            .All(o => o.isComplete))
        {
            OnLevelComplete();
        }
    }

    private void OnLevelFail() 
    {
        GameManager.Instance.RestartLevel();
    }

    private void OnLevelComplete()
    {
        Debug.Log($"{LevelData.LevelName} completed!");
        LevelSelect.Instance.OnLevelComplete(LevelData);
        
        GameManager.Instance.LoadNextScene();
    }

}