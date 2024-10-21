using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UISpeedControl : UIButtonBase
    {
        [SerializeField] private bool TogglePause;

        [SerializeField] private Image _image;
        [SerializeField] private Sprite pauseSprite, resumeSprite;
        [SerializeField] private Sprite OneX, TwoX, FourX;

        public override void OnStart()
        {
            ChangePauseIcon(false);
        }

        private void OnEnable()
        {
            GameSpeed.OnGamePaused += ChangePauseIcon;
        }

        private void OnDisable()
        {
            GameSpeed.OnGamePaused -= ChangePauseIcon;
        }

        public override void OnClick()
        {
            SetSpeed();
        }

        private void ChangePauseIcon(bool isPaused)
        {
            if (!TogglePause) return;
            
            if (isPaused)
            {
                _image.sprite = pauseSprite;
            }
            else
            {
                _image.sprite = resumeSprite;
            }
        }

        public void SetSpeed()
        {
            if (TogglePause)
            {
                if (GameSpeed.Instance.IsGamePaused)
                {
                    GameSpeed.Instance.ResumeGame();
                }
                else
                {
                    GameSpeed.Instance.PauseGame();
                }
            }
            else
            {
                GameSpeed.Instance.CycleSpeed();
                switch (GameSpeed.Instance.GetCurrentSpeed)
                {
                    case GameSpeed.GameSpeedX.X1:
                        _image.sprite = OneX;
                        break;
                    case GameSpeed.GameSpeedX.X2:
                        _image.sprite = TwoX;
                        break;
                    case GameSpeed.GameSpeedX.X4:
                        _image.sprite = FourX;
                        break;
                }
            }
        }
    }
}