using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SoundFXPlayer : MonoBehaviour
    {
        public static SoundFXPlayer Instance;
        public AudioSource Source { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Source = GetComponent<AudioSource>();
        }

        public static void PlaySFX(AudioSource source, AudioClip clip)
        {
            source.PlayOneShot(clip, AudioManager.SFXVolume);
            Debug.Log("ANimatiion Played");
        }
    }
}