using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public BaseObjective[] objectives;

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
        foreach (var obj in objectives) obj.ResetObjective();
    }

    public void OnCargoDelivered(Cargo cargo)
    {
        deliveredCargo.Add(cargo);

        foreach (var obj in objectives)
            obj.AddProgress(cargo);

        if (objectives.All(o => o.isComplete))
            OnLevelComplete();
    }

    public void OnObjectiveComplete()
    {
        if (objectives.All(o => o.isComplete))
            OnLevelComplete();
    }

    private void OnLevelComplete()
    {
        Debug.Log("Level completed!");
        GameManager.Instance.LoadNextScene();
    }


}
