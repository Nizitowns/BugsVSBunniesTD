using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [System.Serializable]
    public class UpgradePathUI
    {
        public Button Btn;
        public TextMeshProUGUI Cost;
        public Image Icon;

        public void ResetGraphics()
        {

            Cost.text = "---";
            Icon.enabled = false;
            Icon.sprite = null;
        }
        public void ResetAll()
        {
            ResetGraphics();
            Btn.interactable = false;
        }
    }
    public UpgradePathUI Upgrade1;
    public UpgradePathUI Upgrade2;

    public void CheckButtons()
    {


        if (TowerPlacer.SelectedPlacedTower!=null)
        {
            if (TowerPlacer.SelectedPlacedTower.UpgradePaths.Length>0)
            {
                Upgrade1.ResetGraphics();

                Upgrade1.Icon.color = Color.Lerp(TowerPlacer.SelectedPlacedTower.UpgradePaths[0].GetComponent<TowerBehavior>().PurchaseIconAdditionalTint, Upgrade1.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade1.Icon.enabled = true;
                Upgrade1.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.SelectedPlacedTower.UpgradePaths[0].GetComponent<TowerBehavior>().PurchaseCost;
                Upgrade1.Icon.sprite = TowerPlacer.SelectedPlacedTower.UpgradePaths[0].GetComponent<TowerBehavior>().PurchaseIcon;
                Upgrade1.Cost.text = TowerPlacer.SelectedPlacedTower.UpgradePaths[0].GetComponent<TowerBehavior>().PurchaseCost + "$";
            }
            else
            {
                Upgrade1.ResetAll();
            }
            if (TowerPlacer.SelectedPlacedTower.UpgradePaths.Length > 1)
            {
                Upgrade2.ResetGraphics();

                
                Upgrade2.Icon.color =Color.Lerp(TowerPlacer.SelectedPlacedTower.UpgradePaths[1].GetComponent<TowerBehavior>().PurchaseIconAdditionalTint,Upgrade2.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade2.Icon.enabled = true;
                Upgrade2.Icon.sprite = TowerPlacer.SelectedPlacedTower.UpgradePaths[1].GetComponent<TowerBehavior>().PurchaseIcon;
                Upgrade2.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.SelectedPlacedTower.UpgradePaths[1].GetComponent<TowerBehavior>().PurchaseCost;
                Upgrade2.Cost.text = TowerPlacer.SelectedPlacedTower.UpgradePaths[1].GetComponent<TowerBehavior>().PurchaseCost + "$";
            }
            else
            {
                Upgrade2.ResetAll();
            }
        }
        else
        {
            Upgrade1.ResetAll();
            Upgrade2.ResetAll();
        }
    }

    public void UpgradeTower(int upgradeSlot)
    {
        TowerBehavior NewUpgrade = TowerPlacer.SelectedPlacedTower.UpgradePaths[upgradeSlot].GetComponent<TowerBehavior>();
        if (MoneyManager.instance.RemoveMoney(NewUpgrade.PurchaseCost))
        {
            TowerBehavior lastTower = TowerPlacer.SelectedPlacedTower;
            GameObject newObj = Instantiate(NewUpgrade.gameObject, lastTower.transform.position, lastTower.transform.rotation, lastTower.transform.parent);

            Destroy(TowerPlacer.SelectedPlacedTower.gameObject);

            TowerPlacer.SelectedTower = null;
            TowerPlacer.SelectedPlacedTower = newObj.GetComponent<TowerBehavior>();
        }
    }
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
        CheckButtons();
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
