using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Time")]
public class TimeObjective : BaseObjective
{
    public float timeLimit = 60f; 

    public override string objectiveName
    {
        get
        {
            return $"Finish level within {UIManager.Instance.StringTime(timeLimit)}";
        }
    }

    public override string objectiveStatus
    {
        get
        {
            float remaining = timeLimit - LevelManager.Instance.elapsedTime;

            if (remaining >= 0f)
                return $"{UIManager.Instance.StringTime(remaining)} remaining";
            else
                return $"{UIManager.Instance.StringTime(-remaining)} over time";
        }
    }


    public override void ResetObjective()
    {
        State = ObjectiveState.InProgress;
    }

    public override void UpdateProgress(Cargo cargo = null, float value = 0f)
    {
        if (State != ObjectiveState.InProgress) return;

        if (value > 0f && value >= timeLimit)
        {
            Fail();
        }

        ObjectiveTracker.Instance.Refresh();
    }

    public void OnLevelComplete()
    {
        if(State == ObjectiveState.InProgress) State = ObjectiveState.Complete;
    }

}
