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
        private GameSpeedX currentSpeed;

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
            if (InputGather.Instance.SpaceButton)
            {
                GameSpeedX[] speeds = (GameSpeedX[])Enum.GetValues(typeof(GameSpeedX));

                int currentIndex = Array.IndexOf(speeds, currentSpeed);
                currentIndex = (currentIndex + 1) % speeds.Length;

                SetGameSpeed(speeds[currentIndex]);
            }
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame(bool setDefault = false)
        {
            if (setDefault)
            {
                currentSpeed = GameSpeedX.X1;
                Time.timeScale = 1;
                return;
            } 
            
            Time.timeScale = (int)currentSpeed;
        }

        public void SetGameSpeed(GameSpeedX speed)
        {
            Time.timeScale = (int)speed;
            currentSpeed = speed;
        }
    }
}