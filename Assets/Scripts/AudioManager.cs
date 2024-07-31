using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static float SFXVolume = 0.5f;
    public static float MusicVolume = 0.5f;


    private void Start()
    {
        SFXVolume = PlayerPrefs.GetFloat("SFX_Vol", 0.5f);
        MusicVolume = PlayerPrefs.GetFloat("Music_Vol", 0.5f);
    }
    public void SetSFXVolume(Slider slider)
    {
        SFXVolume = slider.value/slider.maxValue;

        PlayerPrefs.SetFloat("SFX_Vol", SFXVolume);
    }
    public void SetMusicVolume(Slider slider)
    {
        MusicVolume = slider.value / slider.maxValue;


        PlayerPrefs.SetFloat("Music_Vol", MusicVolume);
    }
}
