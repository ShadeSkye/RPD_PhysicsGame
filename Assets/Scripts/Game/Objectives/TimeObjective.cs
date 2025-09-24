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
        isComplete = false;
        isFailed = false;
    }

    public override void UpdateProgress(Cargo cargo = null, float value = 0f)
    {
        if (isComplete || isFailed) return;

        if (value > 0f && value >= timeLimit)
        {
            Fail();
        }

        ObjectiveTracker.Instance.UpdateText();
    }

    public void OnLevelComplete()
    {
        if(!isFailed) isComplete = true;
    }

}
