using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cargo Objective", menuName = "Objectives/Cargo")]
public class CargoObjective : BaseObjective
{
    public int requiredAmount;
    public CargoType targetType;
    [HideInInspector] public int currentAmount;

    public override void AddProgress(Cargo cargo = null, float value = 0f)
    {
        if (isComplete) return;

        bool cargoValid = (cargo != null && (cargo.type == targetType || cargo.type == CargoType.Any));

        if (!cargoValid) return;

        //Debug.Log("Adding cargo to cargo counter objective");

        currentAmount++;
        if (currentAmount >= requiredAmount)
        {
            currentAmount = requiredAmount;
            isComplete = true;
            Debug.Log($"{objectiveName} completed!");
        }
    }

    public override void ResetObjective()
    {
        currentAmount = 0;
        isComplete = false;
    }
}

