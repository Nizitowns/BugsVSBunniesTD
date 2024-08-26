using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
[RequireComponent(typeof(AudioSource))]
public class AudioHook : MonoBehaviour
{
    float initialVolume;
    AudioSource source;
    public bool FadeInOnStart;
    private void Start()
    {
        source=GetComponent<AudioSource>(); 
        initialVolume =source.volume;

        if(FadeInOnStart)
        {
            source.volume = 0;
        }

        if (audioSourceType == AudioSourceType.SFX)
        {
            source.volume = (initialVolume * AudioManager.SFXVolume);
        }
        else if (audioSourceType == AudioSourceType.Music)
        {
            source.volume = (initialVolume * AudioManager.MusicVolume);
        }
    }
    private void Update()
    {
        if(audioSourceType==AudioSourceType.SFX)
        {
            source.volume = (source.volume*19 +initialVolume * AudioManager.SFXVolume)/20f;
        }
        else if (audioSourceType == AudioSourceType.Music)
        {
            source.volume = (source.volume * 19 + initialVolume * AudioManager.MusicVolume) / 20f;
        }
    }
    public AudioSourceType audioSourceType;
    public enum AudioSourceType {SFX,Music};
}
