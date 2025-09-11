using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    Continuous,
    OneShot,
    UI,
    Music
}

[System.Serializable]
public class AudioID
{
    public string name;
}
[CreateAssetMenu(fileName = "NewAudioData", menuName = "Audio/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioID id;
    public AudioType audioType;
    public AudioMixerGroup mixerGroup;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    public float minVolume = 0f;
    public float maxVolume = 1f;

    public bool restartOnActivation = false;
}
