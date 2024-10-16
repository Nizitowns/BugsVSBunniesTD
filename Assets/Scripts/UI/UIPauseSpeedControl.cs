using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIPauseSpeedControl : UISpeedControl
    {
        [SerializeField] private Sprite pauseSprite, resumeSprite;

        private Image _image;

        public override void OnAwake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            GameSpeed.OnGamePaused += UpdateIcon;
        }

        private void OnDisable()
        {
            GameSpeed.OnGamePaused -= UpdateIcon;
        }

        private void UpdateIcon(bool isPaused)
        {
            if (isPaused)
            {
                _image.sprite = pauseSprite;
                return;
            }

            _image.sprite = resumeSprite;
        }
    }
}