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
    [SerializeField] private AudioSource thrusterSFXSource;
    [SerializeField] private AudioSource boostSFXSource;
    [SerializeField] private AudioSource magnetizeSFXSource;
    [SerializeField] private AudioSource buttonSFXSource;

    public List<AudioClip> SFX = new List<AudioClip>();
    public List<AudioClip> Music = new List<AudioClip>();

    [SerializeField] private float maxVolume = 1f;

    private float previousBoosterVolume = 0f;

    private void Awake()
    {
        Instance = this;

        PlayMusic();
        StartLoopingFX();

        DontDestroyOnLoad(gameObject);
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

        thrusterSFXSource.Play();
        boostSFXSource.Play();

        thrusterSFXSource.volume = 0;
        boostSFXSource.volume = 0;
    }

    public void UpdateThrusterSFX(float intensity)
    {
        float volume = Mathf.Clamp01(intensity) * maxVolume;
        Debug.Log($"[SFX] Thruster intensity {intensity} converted to volume {volume}");
        thrusterSFXSource.volume = volume;
    }

    public void UpdateBoosterSFX(float intensity)
    {
        float volume = Mathf.Clamp01(intensity) * maxVolume;
        Debug.Log($"[SFX] Booster intensity {intensity} converted to volume {volume}");

        if (volume > 0f && previousBoosterVolume == 0f)
        {
            boostSFXSource.Stop();
            boostSFXSource.Play();
        }

        Debug.Log($"[SFX] Booster intensity {intensity} converted to volume {volume}");
        boostSFXSource.volume = volume;

        previousBoosterVolume = volume;
    }




    /*

        public void PlayShipSFX(int I)
        {
            StopShipSFX();
            shipSFXSource.clip = SFX[I];
            shipSFXSource.Play();
        }

        public void PlayMagnetizeSFX(int I)
        {
            StopMagnetizeSFX();
            magnetizeSFXSource.clip = SFX[I];
            magnetizeSFXSource.Play();
        }*/

        public void PlayButtonSFX(int I)
        {
            StopButtonSFX();
            buttonSFXSource.clip = SFX[I];
            buttonSFXSource.Play();
        }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX() => sfxSource.Stop();
    //public void StopShipSFX() => shipSFXSource.Stop();
    public void StopMagnetizeSFX() => magnetizeSFXSource.Stop();
    public void StopButtonSFX() => buttonSFXSource.Stop();

    internal float GetSFXClipLength(int I)
    {
        return SFX[I].length;
    }
}
