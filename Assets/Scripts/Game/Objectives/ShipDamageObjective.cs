using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/ShipDamage")]
public class ShipDamageObjective : BaseObjective
{
    public float damageThreshold;

    public override string objectiveName
    {
        get
        {

            string text = $"Complete level with less than {damageThreshold}% ship damage";

            if (objectiveValue > 0) text += $" [{CurrencyManager.Instance.CurrencyFormatted(objectiveValue)}]";

            return text;
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
        State = ObjectiveState.InProgress;
    }

    public override void UpdateProgress(Cargo cargo = null, float value = 0f)
    {
        if (State != ObjectiveState.InProgress) return;

        if (PlayerManager.Instance.damagePercent > damageThreshold+1)
        {
            Fail();
        }

        ObjectiveTracker.Instance.Refresh();
    }

    public void OnLevelComplete()
    {
        if (State != ObjectiveState.Failed)
        {
            Complete();
        }
    }
}
