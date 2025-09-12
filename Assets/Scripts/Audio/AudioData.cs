using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioData : ScriptableObject
{
    public AudioMixerGroup mixerGroup;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    
}