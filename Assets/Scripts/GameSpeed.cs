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
        private GameSpeedX lastSpeed;
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
            currentSpeed = GameSpeedX.X1;
        }

        private void Update()
        {
            if (InputGather.Instance.SpaceButton && !hardPause)
            {
                GameSpeedX[] speeds = (GameSpeedX[])Enum.GetValues(typeof(GameSpeedX));

                int currentIndex = Array.IndexOf(speeds, currentSpeed);
                currentIndex = (currentIndex + 1) % speeds.Length;

                SetGameSpeed(speeds[currentIndex]);
            }
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

        public GameSpeedX GetCurrentSpeed => currentSpeed;
    }
}