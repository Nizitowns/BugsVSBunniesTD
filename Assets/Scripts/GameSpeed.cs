using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameSpeed : MonoBehaviour
    {
        public enum GameSpeedX
        {
            X0 = 0,
            X1 = 1,
            X2 = 2,
            X4 = 4,
        }
        
        public static GameSpeed Instance;
        private GameSpeedX lastSpeed = GameSpeedX.X1;
        private GameSpeedX currentSpeed;

        private bool hardPause = false;
        
        public bool IsHardPaused
        {
            get { return hardPause; }
        }

        public static event Action<bool> OnGamePaused;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetGameSpeed(GameSpeedX.X1);
        }

        public void PauseGame(bool hardPause = false)
        {
            currentSpeed = GameSpeedX.X0;
            Time.timeScale = 0;
            
            if (hardPause)
                this.hardPause = true;
            
            OnGamePaused?.Invoke(true);
        }

        public void ResumeGame(bool setDefault = false, bool breakHardPause = false)
        {
            if (breakHardPause)
            {
                hardPause = false;
            }
            else if(hardPause)
            {
                return;
            }

            OnGamePaused?.Invoke(false);
            
            if (setDefault)
            {
                currentSpeed = GameSpeedX.X1;
                Time.timeScale = 1;
                return;
            }
            
            Time.timeScale = (int)lastSpeed;
            currentSpeed = lastSpeed;
        }

        public void SetGameSpeed(GameSpeedX speed)
        {
            if (hardPause) return;
            
            OnGamePaused?.Invoke(false);
            
            Time.timeScale = (int)speed;
            currentSpeed = speed;
            lastSpeed = currentSpeed;
        }

        public void CycleSpeed()
        {
            switch (currentSpeed)
            {
                case GameSpeedX.X1:
                    SetGameSpeed(GameSpeedX.X2);
                    break;
                case GameSpeedX.X2:
                    SetGameSpeed(GameSpeedX.X4);
                    break;
                case GameSpeedX.X4:
                    SetGameSpeed(GameSpeedX.X1);
                    break;
            }
        }

        public GameSpeedX GetCurrentSpeed => currentSpeed;

        public bool IsGamePaused => currentSpeed == GameSpeedX.X0;
    }
}