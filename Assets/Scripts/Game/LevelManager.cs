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

        if (LevelData.objectives.All(o => o.isComplete))
            OnLevelComplete();
    }

    public void OnObjectiveComplete()
    {
        if (LevelData.objectives.All(o => o.isComplete))
            OnLevelComplete();
    }

    private void OnLevelComplete()
    {
        Debug.Log($"{LevelData.LevelName} completed!");
        LevelSelect.Instance.OnLevelComplete(LevelData);
        
        GameManager.Instance.LoadNextScene();
    }

}