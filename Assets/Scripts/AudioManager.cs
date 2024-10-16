using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static float SFXVolume = 0.5f;
    public static float MusicVolume = 0.5f;

    public Slider SFXSlider;
    public Slider MusicSlider;
    private void Start()
    {
        SFXVolume = PlayerPrefs.GetFloat("SFX_Vol", 0.5f);
        MusicVolume = PlayerPrefs.GetFloat("Music_Vol", 0.5f);
        if(SFXSlider!=null)
            SFXSlider.value = SFXVolume;
        if(MusicSlider!=null)
            MusicSlider.value = MusicVolume;

    }
    public void SetSFXVolume(Slider slider)
    {
        SFXVolume = slider.value/slider.maxValue;

        PlayerPrefs.SetFloat("SFX_Vol", SFXVolume);
        SFXVolume = Mathf.Log10((SFXVolume) * 20);
    }
    public void SetMusicVolume(Slider slider)
    {
        MusicVolume = slider.value / slider.maxValue;

        PlayerPrefs.SetFloat("Music_Vol",MusicVolume);
        MusicVolume = Mathf.Log10((MusicVolume) * 20);
    }
}
