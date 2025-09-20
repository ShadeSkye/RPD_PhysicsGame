using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level Data", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public SceneIndex SceneIndex;
    public string LevelName;
    public BaseObjective[] objectives;

}
