using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public LevelData LevelData;
    public List<Cargo> deliveredCargo = new List<Cargo>();

    public float elapsedTime;

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

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        foreach (var o in LevelData.objectives.OfType<TimeObjective>())
        {
            o.UpdateProgress(value: elapsedTime);
        }
    }

    public void OnCargoDelivered(Cargo c)
    {
        deliveredCargo.Add(c);

        foreach (var o in LevelData.objectives.OfType<CargoObjective>())
            o.UpdateProgress(cargo: c);

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
        List<BaseObjective> critical = new List<BaseObjective>();
        int completedCriticalCount = 0;

        foreach (BaseObjective o in LevelData.objectives)
        {
            if (o.isCritical) critical.Add(o);
        }

        bool anyFailed = false;

        foreach (BaseObjective c in critical)
        {
            if (c.isFailed) anyFailed = true;
            else if (c.isComplete) completedCriticalCount += 1;
        }

        if (anyFailed)
        {
            OnLevelFail();
            return; 
        }

        if (completedCriticalCount == critical.Count && critical.Count > 0)
        {
            OnLevelComplete();
        }
    }


    private void OnLevelFail() 
    {
        Debug.Log($"{LevelData.LevelName} restarting...");
        GameManager.Instance.RestartLevel();
    }

    private void OnLevelComplete()
    {
        foreach (var o in LevelData.objectives.OfType<TimeObjective>())
        {
            o.OnLevelComplete();
        }

        ObjectiveOverview();

        Debug.Log($"{LevelData.LevelName} completed!");
        LevelSelect.Instance.OnLevelComplete(LevelData);
        
        GameManager.Instance.LoadNextScene();
    }

    private void ObjectiveOverview()
    {
        string txt = "";

        foreach (var o in LevelData.objectives)
        {
            string status = o.isComplete ? "succeeded" : o.isFailed ? "failed" : "in progress";
            string prefix = o.isCritical ? "[C] " : "";
            txt += $"{prefix}{o.objectiveName}: {status}\n";
        }

        Debug.Log(txt);
    }

}