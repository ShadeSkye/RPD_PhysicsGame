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
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource rotateSFXSource;
    [SerializeField] private AudioSource thrusterSFXSource;
    [SerializeField] private AudioSource boostSFXSource;
    [SerializeField] private AudioSource buttonSFXSource;

    public List<AudioClip> SFX = new List<AudioClip>();
    public List<AudioClip> Music = new List<AudioClip>();

    [SerializeField] private float maxVolume = 1f;

    private float targetThrusterVolume = 0f;
    private float targetBoosterVolume = 0f;
    private float targetRotationVolume = 0f;

    private float previousBoosterVolume = 0f;
    [SerializeField] private float fadeSpeed = 3f;
    private void Awake()
    {
        Instance = this;

        PlayMusic();
        StartLoopingFX();

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        thrusterSFXSource.volume = Mathf.MoveTowards(
            thrusterSFXSource.volume,
            targetThrusterVolume,
            fadeSpeed * Time.deltaTime
        );

        boostSFXSource.volume = Mathf.MoveTowards(
            boostSFXSource.volume,
            targetBoosterVolume,
            fadeSpeed * Time.deltaTime
        );

        rotateSFXSource.volume = Mathf.MoveTowards(
            rotateSFXSource.volume,
            targetRotationVolume,
            fadeSpeed * Time.deltaTime
        );
    }

    public void PlayMusic()
    {
        musicSource.clip = Music[0];
        musicSource.Play();
    }

    public void StartLoopingFX()
    {
        thrusterSFXSource.clip = SFX[0];
        boostSFXSource.clip = SFX[1];
        rotateSFXSource.clip = SFX[4];

        thrusterSFXSource.Play();
        boostSFXSource.Play();
        rotateSFXSource.Play();

        thrusterSFXSource.volume = 0;
        boostSFXSource.volume = 0;
        rotateSFXSource.volume = 0;
    }

    public void UpdateThrusterSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0f, 1f) * maxVolume;
        Debug.Log($"[SFX] Thruster intensity {intensity} converted to volume {volume}");
        targetThrusterVolume = volume;
    }

    public void UpdateRotateSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0.2f, 1f) * maxVolume;
        Debug.Log($"[SFX] Rotate intensity {intensity} converted to volume {volume}");
        targetRotationVolume = volume;
    }

    public void UpdateBoosterSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0f, 1f) * maxVolume;
        Debug.Log($"[SFX] Booster intensity {intensity} converted to volume {volume}");

        if (volume > 0f && previousBoosterVolume == 0f)
        {
            boostSFXSource.Stop();
            boostSFXSource.Play();
        }

        Debug.Log($"[SFX] Booster intensity {intensity} converted to volume {volume}");
        targetBoosterVolume = volume;

        previousBoosterVolume = volume;
    }

    public void PlaySFX(int I)
    {
        StopSFX();
        sfxSource.clip = SFX[I];
        sfxSource.Play();
    }

    public void PlayButtonSFX(int I)
        {
            StopButtonSFX();
            buttonSFXSource.clip = SFX[I];
            buttonSFXSource.Play();
        }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX()
    {
        sfxSource.Stop();
        thrusterSFXSource.Stop();
        boostSFXSource.Stop();
        rotateSFXSource.Stop();
    }
    public void StopButtonSFX() => buttonSFXSource.Stop();

    internal float GetSFXClipLength(int I)
    {
        return SFX[I].length;
    }
}
