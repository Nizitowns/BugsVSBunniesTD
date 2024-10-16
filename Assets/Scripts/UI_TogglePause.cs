using UnityEngine;

namespace DefaultNamespace
{
    public class UI_TogglePause : MonoBehaviour
    {
        private void OnEnable()
        {
            GameSpeed.Instance.PauseGame(hardPause:true);
        }

        private void OnDisable()
        {
            GameSpeed.Instance.ResumeGame(breakHardPause:true);
        }

        private void OnDestroy()
        {
            GameSpeed.Instance.ResumeGame(breakHardPause:true);
        }
    }
}