using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLink : MonoBehaviour
{
    public enum SliderTypes { Music, SFX, Master, Sensitivity }
    public SliderTypes type;

    private void Awake()
    {
        Slider slider = GetComponent<Slider>();
        if (slider == null || VolumeSettings.instance == null)
            return;

        switch (type)
        {
            case SliderTypes.Music:
                slider.onValueChanged.AddListener((v) => VolumeSettings.instance.SetMusicVolume());
                break;
            case SliderTypes.SFX:
                slider.onValueChanged.AddListener((v) => VolumeSettings.instance.SetSFXVolume());
                break;
            case SliderTypes.Master:
                slider.onValueChanged.AddListener((v) => VolumeSettings.instance.SetMasterVolume());
                break;
            case SliderTypes.Sensitivity:
                break;
        }
    }
}
