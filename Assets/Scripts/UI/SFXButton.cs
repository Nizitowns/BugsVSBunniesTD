using Helper;
using UnityEngine;

namespace DefaultNamespace
{
    public class SFXButton : UIButtonBase
    {
        [SerializeField] private AudioClip _soundClip;

        private AudioSource _source;
        
        public override void OnStart()
        {
            _source = ComponentCopier.CopyComponent(SoundFXPlayer.Source, transform.gameObject);
        }

        public override void OnClick()
        {
            SoundFXPlayer.PlaySFX(_source, _soundClip, 2);
        }
    }
}