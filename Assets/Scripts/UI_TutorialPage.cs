using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class UI_TutorialPage : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Pages;
        private int currentIndex = 0;

        [SerializeField] private GameObject LeftButton;
        [SerializeField] private GameObject RighButton;
        
        private void OnEnable()
        {
            ToggleButtons();
            CloseAll();
            Pages[0].SetActive(true);
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