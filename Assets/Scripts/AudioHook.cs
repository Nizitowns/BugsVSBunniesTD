using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
[RequireComponent(typeof(AudioSource))]
public class AudioHook : MonoBehaviour
{
    float initialVolume;
    AudioSource source;
    private void Start()
    {
        source=GetComponent<AudioSource>(); 
        initialVolume =source.volume;
    }
    private void Update()
    {
        if(audioSourceType==AudioSourceType.SFX)
        {
            source.volume = initialVolume * AudioManager.SFXVolume;
        }
        else if (audioSourceType == AudioSourceType.Music)
        {
            source.volume = initialVolume * AudioManager.MusicVolume;
        }
    }
    public AudioSourceType audioSourceType;
    public enum AudioSourceType {SFX,Music};
}
