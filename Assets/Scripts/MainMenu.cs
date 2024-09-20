using UnityEngine;

namespace DefaultNamespace
{
    public class MainMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            GameSpeed.Instance.PauseGame();
        }

        private void OnDisable()
        {
            GameSpeed.Instance.ResumeGame();
        }

        private void OnDestroy()
        {
            GameSpeed.Instance.ResumeGame();
        }
    }
}