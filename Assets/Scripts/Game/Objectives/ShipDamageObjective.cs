using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/ShipDamage")]
public class ShipDamageObjective : BaseObjective
{
    public float damageThreshold;

    public override string objectiveName
    {
        get
        {
            return $"Complete level with less than {damageThreshold}% ship damage";
        }
    }

    public override string objectiveStatus
    {
        get
        {
            return $"Taken {PlayerManager.Instance.damagePercent*100:F0}% damage";
        }
    }


    public override void ResetObjective()
    {
        isComplete = false;
        isFailed = false;
    }

    public override void UpdateProgress(Cargo cargo = null, float value = 0f)
    {
        if (isComplete || isFailed) return;

        if (PlayerManager.Instance.damagePercent > damageThreshold+1)
        {
            Fail();
        }

        ObjectiveTracker.Instance.UpdateText();
    }

    public void OnLevelComplete()
    {
        if (!isFailed)
        {
            Complete();
        }
    }
}
