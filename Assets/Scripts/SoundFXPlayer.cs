using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SoundFXPlayer : MonoBehaviour
    {
        public static SoundFXPlayer Instance;
        public static AudioSource Source { get; private set; }

        private void Awake()
        {
            Instance = this;
            Source = GetComponent<AudioSource>();
        }

        public static void PlaySFX(AudioSource source, AudioClip clip, float volumeMultiplier = 1)
        {
            source.PlayOneShot(clip, AudioManager.SFXVolume * volumeMultiplier);
        }
    }
}