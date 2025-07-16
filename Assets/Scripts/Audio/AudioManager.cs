using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource shipSFXSource;
    [SerializeField] private AudioSource magnetizeSFXSource;

    public List<AudioClip> SFX = new List<AudioClip>();
    public List<AudioClip> Music = new List<AudioClip>();

    private void Awake()
    {
        instance = this;
        musicSource.clip = Music[0];
        PlayMusic();
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

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
    }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX() => sfxSource.Stop();
    public void StopShipSFX() => shipSFXSource.Stop();
    public void StopMagnetizeSFX() => magnetizeSFXSource.Stop();
}
