using UnityEngine;

namespace DefaultNamespace
{
    public class UISpeedControl : UIButtonBase
    {
        [SerializeField] private GameSpeed.GameSpeedX SpeedToSet;

        public override void OnClick()
        {
            SetSpeed();
        }

        public virtual void SetSpeed()
        {
            switch (SpeedToSet)
            {
                case GameSpeed.GameSpeedX.X0:
                    if(GameSpeed.Instance.IsGamePaused)
                        GameSpeed.Instance.ResumeGame();
                    else
                        GameSpeed.Instance.PauseGame();

                    break;
                case GameSpeed.GameSpeedX.X1:
                    GameSpeed.Instance.ResumeGame(true);
                    break;
                case GameSpeed.GameSpeedX.X2:
                case GameSpeed.GameSpeedX.X4:
                    GameSpeed.Instance.SetGameSpeed(SpeedToSet);
                    break;
            }
        }
    }
}