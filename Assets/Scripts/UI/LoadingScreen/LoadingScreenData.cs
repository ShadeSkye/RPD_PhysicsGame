using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Loading Screen")]
public class LoadingScreenData : ScriptableObject
{
    [Header("Visuals")]
    public Sprite backgroundImage;
    [TextArea(5,5)] public string loadingText = "Loading...";

    [Header("Options")]
    public float holdTime = 2f;
    public bool requireContinueButton = true;
}

