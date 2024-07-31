using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static float SFXVolume = 0.5f;

    public void SetVolume(Slider slider)
    {
        SFXVolume = slider.value/slider.maxValue;
    }
}
