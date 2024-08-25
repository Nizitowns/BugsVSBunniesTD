using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup=GetComponent<CanvasGroup>();
    }

    void Update()
    {
        canvasGroup.interactable = canvasGroup.alpha > 0;
        canvasGroup.blocksRaycasts = canvasGroup.alpha > 0;
        canvasGroup.alpha = TowerPlacer.SelectedPlacedTower == null ? 0 : 1;
    }
}
