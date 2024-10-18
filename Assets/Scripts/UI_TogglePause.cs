using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class UI_TogglePause : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Pages;
        private int currentIndex = 0;

        [SerializeField] private GameObject LeftButton;
        [SerializeField] private GameObject RighButton;
        
        private void OnEnable()
        {
            GameSpeed.Instance.PauseGame(hardPause:true);
            ToggleButtons();
            CloseAll();
            Pages[0].SetActive(true);
        }

        private void OnDisable()
        {
            GameSpeed.Instance.ResumeGame(breakHardPause:true);
        }

        private void OnDestroy()
        {
            GameSpeed.Instance.ResumeGame(breakHardPause:true);
        }

        public void GoNext()
        {
            if (currentIndex >= Pages.Count - 1) return;
            
            CloseAll();
            currentIndex++;
            ToggleButtons();
            Pages[currentIndex].SetActive(true);
        }

        public void GoPrevious()
        {
            if (currentIndex <= 0) return;
            
            CloseAll();
            currentIndex--;
            ToggleButtons();
            Pages[currentIndex].SetActive(true);
        }

        private void CloseAll()
        {
            foreach (var page in Pages)
            {
                page.gameObject.SetActive(false);
            }
        }

        private void ToggleButtons()
        {
            if(currentIndex <= 0) LeftButton.SetActive(false);
            else LeftButton.SetActive(true);

            if (currentIndex >= Pages.Count - 1) RighButton.SetActive(false);
            else RighButton.SetActive(true);
        }
    }
}