using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level Data", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [HideInInspector] public SceneIndex SceneIndex { get; internal set; }
    public string LevelName;
    public BaseObjective[] objectives;
    public BaseObjective exitLevelObjective;
    public LoadingScreenData loadingScreen;

}
