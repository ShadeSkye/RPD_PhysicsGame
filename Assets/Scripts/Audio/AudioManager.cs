using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public enum ContinuousSFX
{
    Thrust,
    Boost,
    Rotation,
    Magnetize

}

public enum OneShotSFX
{
    Lock,
    Eject,
    CrateHit,
    Deposited
}

public enum UISFX
{
    Button

}

public enum MusicClips
{
    Main
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource rotateSFXSource;
    [SerializeField] private AudioSource thrusterSFXSource;
    [SerializeField] private AudioSource magnetizeSource;
    [SerializeField] private AudioSource boostSFXSource;
    [SerializeField] private AudioSource buttonSFXSource;


    [Header("Assign Clips")]
    public AudioClip[] continuousSFX; 
    public AudioClip[] oneShotSFX;
    public AudioClip[] uiSFX;
    public AudioClip[] musicClips;

    [SerializeField] private float maxVolume = 1f;

    private float targetThrusterVolume = 0f;
    private float targetBoosterVolume = 0f;
    private float targetRotationVolume = 0f;
    private float targetMagnetizeSource = 0f;

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

        magnetizeSource.volume = Mathf.MoveTowards(
            magnetizeSource.volume,
            targetMagnetizeSource,
            fadeSpeed * Time.deltaTime
        );
    }

    public void PlayMusic()
    {
        musicSource.clip = musicClips[(int)MusicClips.Main];
        musicSource.Play();
    }

    public void StartLoopingFX()
    {
        thrusterSFXSource.clip = continuousSFX[(int)ContinuousSFX.Thrust];
        boostSFXSource.clip = continuousSFX[(int)ContinuousSFX.Boost];
        rotateSFXSource.clip = continuousSFX[(int)ContinuousSFX.Rotation];
        magnetizeSource.clip = continuousSFX[(int)ContinuousSFX.Magnetize];

        thrusterSFXSource.Play();
        boostSFXSource.Play();
        rotateSFXSource.Play();
        magnetizeSource.Play();

        thrusterSFXSource.volume = 0;
        boostSFXSource.volume = 0;
        rotateSFXSource.volume = 0;
        magnetizeSource.volume = 0;
    }

    public void UpdateThrusterSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0f, 1f) * maxVolume;
        //Debug.Log($"[SFX] Thruster intensity {intensity} converted to volume {volume}");
        targetThrusterVolume = volume;
    }

    public void UpdateRotateSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0.2f, 1f) * maxVolume;
        //Debug.Log($"[SFX] Rotate intensity {intensity} converted to volume {volume}");
        targetRotationVolume = volume;
    }
    public void UpdateMagnetizeSFX(bool isPulling) 
    {
        float volume;
        if (isPulling)
        {
            volume = maxVolume;
        }
        else
        {
            volume = 0f;
        }

        bool isPlayingAlready = (magnetizeSource.isPlaying && magnetizeSource.clip == continuousSFX[(int)ContinuousSFX.Magnetize]);
        // Debug.Log($"[SFX] Beam is playing? {isPulling} converted to volume {volume}");
        targetMagnetizeSource = volume;
    }

    public void UpdateBoosterSFX(float intensity)
    {
        float volume = Mathf.Clamp(intensity, 0f, 1f) * maxVolume;

        if (volume > 0f && previousBoosterVolume == 0f)
        {
            boostSFXSource.Stop();
            boostSFXSource.Play();
        }

        //Debug.Log($"[SFX] Booster intensity {intensity} converted to volume {volume}");
        targetBoosterVolume = volume;

        previousBoosterVolume = volume;
    }

    public void PlaySFX(OneShotSFX sfx)
    {
        StopSFX();
        sfxSource.clip = oneShotSFX[(int)sfx];
        sfxSource.Play();
    }

    public void PlayButtonSFX(UISFX uisfx)
    {
        StopButtonSFX();
        buttonSFXSource.clip = uiSFX[(int)uisfx];
       buttonSFXSource.Play();
    }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX()
    {
        sfxSource.Stop();
        thrusterSFXSource.Stop();
        boostSFXSource.Stop();
        rotateSFXSource.Stop();
        magnetizeSource.Stop();
    }
    public void StopButtonSFX() => buttonSFXSource.Stop();

}
