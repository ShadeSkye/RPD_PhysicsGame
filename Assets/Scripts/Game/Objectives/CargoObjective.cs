using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cargo Objective", menuName = "Objectives/Cargo")]
public class CargoObjective : BaseObjective
{
    public int requiredAmount;
    public CargoType targetType;
    [HideInInspector] public int currentAmount;

    public override string objectiveName
    {
        get
        {
            string text;

            if(targetType == CargoType.Any)
            {
                text = "of any cargo";
            }
            else
            {
                if (requiredAmount > 1)
                {
                    text = targetType.ToString() + "s";
                }
                else
                {
                    text = targetType.ToString();
                }
            }

            return $"Deliver {requiredAmount} {text}";
        }
    }

    public override string objectiveStatus
    {
        get
        {
            string text;

            if (targetType == CargoType.Any)
            {
                text = "cargo";
            }
            else
            {
                if (requiredAmount > 1)
                {
                    text = targetType.ToString() + "s";
                }
                else
                {
                    text = targetType.ToString();
                }
            }

            return $"{currentAmount}/{requiredAmount} {text} delivered";
        }
    }

    public override void UpdateProgress(Cargo cargo = null, float value = 0f)
    {
        if (isComplete) return;

        bool cargoValid = (cargo != null && (cargo.type == targetType || cargo.type == CargoType.Any));

        if (!cargoValid) return;

        //Debug.Log("Adding cargo to cargo counter objective");

        currentAmount++;
        if (currentAmount >= requiredAmount)
        {
            currentAmount = requiredAmount;
            Debug.Log($"{objectiveName} completed!");

            Complete();
        }
    }

    public override void ResetObjective()
    {
        currentAmount = 0;
        isComplete = false;
        isFailed = false;
    }
}

