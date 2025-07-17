using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterVolumeSlider;

    public static VolumeSettings instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetReferences();

        if (musicSlider != null)
        {
            if (PlayerPrefs.HasKey("musicVolume"))
                LoadMusicVolume();
            else
                SetMusicVolume();
        }

        if (sfxSlider != null)
        {
            if (PlayerPrefs.HasKey("sfxVolume"))
                LoadSFXVolume();
            else
                SetSFXVolume();
        }

        if (masterVolumeSlider != null)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
                LoadMasterVolume();
            else
                SetMasterVolume();
        }
    }

    private void Start()
    {
        GetReferences();

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }
    }
    public void SetMusicVolume()
    {
        if (musicSlider == null) return;

        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadMusicVolume()
    {
        if (musicSlider == null) return;

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
    }

    public void SetSFXVolume()
    {
        if (sfxSlider == null) return;

        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadSFXVolume()
    {
        if (sfxSlider == null) return;

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        if (masterVolumeSlider == null) return;

        float volume = masterVolumeSlider.value;
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void LoadMasterVolume()
    {
        if (masterVolumeSlider == null) return;

        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetMasterVolume();
    }

    public void GetReferences()
    {
        var allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (var slider in allSliders)
        {
            if(slider.name == "MusicSlider")
                musicSlider = slider;

            if (slider.name == "SFXSlider")
                sfxSlider = slider;

            if (slider.name == "MasterVolumeSlider")
                masterVolumeSlider = slider;
        }
    }
}
