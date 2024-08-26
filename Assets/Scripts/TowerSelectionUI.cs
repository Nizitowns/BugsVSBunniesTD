using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    public float RefundRatio = 0.75f;
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup=GetComponent<CanvasGroup>();
        canvas=transform.parent.GetComponent<Canvas>();
    }
    Canvas canvas;
    public TextMeshProUGUI TowerTitle;

    public void SellTower()
    {
        MoneyManager.instance.AddMoney(Mathf.RoundToInt(TowerPlacer.SelectedPlacedTower.PurchaseCost* RefundRatio));
        Destroy(TowerPlacer.SelectedPlacedTower.gameObject);
    }

    //Code Ripped Straight From the Internet :3
    public void AnchorPos(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        // Step 1: Get the screen position of the target object
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position);

        // Step 2: Convert screen position to canvas space (assuming a Screen Space - Overlay canvas)
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out canvasPosition);

        // Step 3: Update the UI element's position
        GetComponent<RectTransform>().anchoredPosition = canvasPosition;
    }
    void Update()
    {
        canvasGroup.interactable = canvasGroup.alpha > 0;
        canvasGroup.blocksRaycasts = canvasGroup.alpha > 0;
        canvasGroup.alpha = TowerPlacer.SelectedPlacedTower == null ? 0 : 1;

        if(TowerPlacer.SelectedPlacedTower!=null)
        {
            TowerTitle.text = TowerPlacer.SelectedPlacedTower.SelectedTitle;
            AnchorPos(TowerPlacer.SelectedPlacedTower.gameObject);
        }
    }
}
