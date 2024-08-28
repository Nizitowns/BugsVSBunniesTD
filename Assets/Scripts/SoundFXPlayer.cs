using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SoundFXPlayer : MonoBehaviour
    {
        public static SoundFXPlayer Instance;
        private AudioSource _source;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlaySFX(AudioClip clip)
        {
            _source.PlayOneShot(clip, AudioManager.SFXVolume);
        }
    }
}