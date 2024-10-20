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
            if (TogglePause)
            {
                _image.sprite = resumeSprite;
            }
            else
            {
                _image.sprite = OneX;
            }

        }

        public override void OnClick()
        {
            SetSpeed();
        }

        public void SetSpeed()
        {
            if (TogglePause)
            {
                if (GameSpeed.Instance.IsGamePaused)
                {
                    GameSpeed.Instance.ResumeGame();
                    _image.sprite = resumeSprite;
                }
                else
                {
                    GameSpeed.Instance.PauseGame();
                    _image.sprite = pauseSprite;
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