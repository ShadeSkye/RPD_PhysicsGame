using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Exit Objective", menuName = "Objectives/Exit")]
public class ExitLevelObjective : BaseObjective
{
    public override string objectiveName => "Travel to Next Level";
    public override string objectiveStatus => "Enter hangar to select next level to travel to";

    public override void ResetObjective()
    {
    }
}
