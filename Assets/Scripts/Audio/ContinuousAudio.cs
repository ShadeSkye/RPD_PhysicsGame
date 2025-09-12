using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "ContinuousAudioData", menuName = "Audio/Continuous")]
public class ContinuousAudio : AudioData
{
    public float minVolume = 0f;
    public float maxVolume = 1f;

    public bool restartOnActivation = false;
}