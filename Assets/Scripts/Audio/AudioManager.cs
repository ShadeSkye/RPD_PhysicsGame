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

    //[SerializeField] private float defaultVolume = 1f;
    //[SerializeField] private float maxVolume = 1f;

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

        AudioSetup();
    }
    private void AudioSetup()
    {

        foreach (AudioData sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = false;
            source.loop = (sound is ContinuousAudio);
            source.volume = (sound is OneShotAudio || sound.mixerGroup.name == "Music") ? 1f : 0f;
            source.outputAudioMixerGroup = sound.mixerGroup;
            sound.source = source;

            audioLookup[sound.name] = sound;

            if (sound is ContinuousAudio)
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

        if (sound is not ContinuousAudio csound) return;

        float volume = Mathf.Clamp(intensity, 0f, 1f) * (csound.maxVolume - csound.minVolume) + csound.minVolume;

        if (csound.restartOnActivation && volume > 0f && csound.source.volume == 0f)
        {
            csound.source.Stop();
            csound.source.Play();
        }

        targetVolumes[csound] = volume;
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

    public void PlayPositional(string soundName, Vector3 position)
    {
        if (!audioLookup.TryGetValue(soundName, out AudioData sound))
        {
            Debug.LogWarning($"Audio key not found: {soundName}");
            return;
        }

        GameObject temp = new GameObject($"TempAudio_{soundName}");
        temp.transform.position = position;

        AudioSource tempSource = temp.AddComponent<AudioSource>();
        tempSource.clip = sound.clip;
        tempSource.spatialBlend = 1f;
        tempSource.minDistance = 50f;
        tempSource.maxDistance = 150f;
        tempSource.rolloffMode = AudioRolloffMode.Logarithmic;

        tempSource.Play();
        Destroy(temp, sound.clip.length);
    }

    public void PauseSFX()
    {
        audioMixer.SetFloat("sfx", -80f);
    }

    public void ResumeSFX()
    {
        audioMixer.SetFloat("sfx", 0f);
    }

    public void StopAllSFX()
    {
        foreach (var sound in sounds)
        {
            if (sound is ContinuousAudio csound)
            {
                csound.source.Stop();

            }
        }
    }

    public void PlayAllSFX()
    {
        foreach (var sound in sounds)
        {
            if (sound is ContinuousAudio csound)
            {
                if (!csound.source.isPlaying)
                    csound.source.Play();
            }
        }
    }
}