using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private List<AudioData> sounds;

    [SerializeField] private float defaultVolume = 1f;
    [SerializeField] private float maxVolume = 1f;

    [SerializeField] private float fadeSpeed = 3f;

    private Dictionary<AudioData, float> targetVolumes = new Dictionary<AudioData, float>();

    public Dictionary<string, AudioData> audioLookup = new Dictionary<string, AudioData>(); 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        AudioSetup();
    }
    private void AudioSetup()
    {
        foreach (AudioData sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = false;
            source.loop = (sound.audioType == AudioType.Continuous || sound.audioType == AudioType.Music);
            source.volume = (sound.audioType == AudioType.Music) ? defaultVolume : 0f;

            source.outputAudioMixerGroup = sound.mixerGroup;
            sound.source = source;

            audioLookup[sound.id.name] = sound;

            if (sound.audioType == AudioType.Continuous || sound.audioType == AudioType.Music)
                source.Play();
        }
    }
    private void Update()
    {
        foreach (var kvp in targetVolumes)
        {
            kvp.Key.source.volume = Mathf.MoveTowards(kvp.Key.source.volume, kvp.Value, fadeSpeed * Time.deltaTime);
        }
    }

    public void UpdateContinuous(string soundName, float intensity)
    {
        if (!audioLookup.TryGetValue(soundName, out AudioData sound))
        {
            Debug.LogWarning($"Audio key not found: {soundName}");
            return;
        }

        float volume = Mathf.Clamp(intensity, 0f, 1f) * (sound.maxVolume - sound.minVolume) + sound.minVolume;

        if (sound.restartOnActivation && volume > 0f && sound.source.volume == 0f)
        {
            sound.source.Stop();
            sound.source.Play();
        }

        targetVolumes[sound] = volume;
    }

    public void PlayOneShot(string soundName)
    {
        if (!audioLookup.TryGetValue(soundName, out AudioData sound))
        {
            Debug.LogWarning($"Audio key not found: {soundName}");
            return;
        }

        sound.source.PlayOneShot(sound.clip);
    }

    public void PauseSFX()
    {
        audioMixer.SetFloat("sfx", -80f);
    }

    public void ResumeSFX()
    {
        audioMixer.SetFloat("sfx", 0f);
    }
}