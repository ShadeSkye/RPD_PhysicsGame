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
            return $"{UIManager.Instance.StringTime(timeLimit-LevelManager.Instance.elapsedTime)} remaining";
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

        ObjectiveTracker.Instance.RefreshButtons();
    }

    public void OnLevelComplete()
    {
        State = ObjectiveState.Complete;
    }

}
