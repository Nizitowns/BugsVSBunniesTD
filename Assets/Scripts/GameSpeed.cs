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
            X5 = 5,
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

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = (int)currentSpeed;
        }

        public void SetGameSpeed(GameSpeedX speed)
        {
            Time.timeScale = (int)speed;
            currentSpeed = speed;
        }
    }
}