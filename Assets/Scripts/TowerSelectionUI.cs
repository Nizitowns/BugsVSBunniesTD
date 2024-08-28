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


        if (TowerPlacer.SelectedPlacedDefaultTower!=null)
        {
            if (TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers.Length>0)
            {
                Upgrade1.ResetGraphics();

                Upgrade1.Icon.color = Color.Lerp(TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[0].purchaseIconAdditionalTint, Upgrade1.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade1.Icon.enabled = true;
                Upgrade1.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[0].purchaseCost;
                Upgrade1.Icon.sprite = TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[0].purchaseIcon;
                Upgrade1.Cost.text = TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[0].purchaseCost + "$";
            }
            else
            {
                Upgrade1.ResetAll();
            }
            if (TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers.Length > 1)
            {
                Upgrade2.ResetGraphics();

                
                Upgrade2.Icon.color =Color.Lerp(TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[1].purchaseIconAdditionalTint,Upgrade2.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade2.Icon.enabled = true;
                Upgrade2.Icon.sprite = TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[1].purchaseIcon;
                Upgrade2.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[1].prefab.GetComponent<DefaultTowerBehaviour>().Config.purchaseCost;
                Upgrade2.Cost.text = TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[1].purchaseCost + "$";
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
        DefaultTowerBehaviour NewUpgrade = TowerPlacer.SelectedPlacedDefaultTower.Config.upgradableTowers[upgradeSlot].prefab.GetComponent<DefaultTowerBehaviour>();
        if (MoneyManager.instance.RemoveMoney(NewUpgrade.Config.purchaseCost))
        {
            DefaultTowerBehaviour lastDefaultTower = TowerPlacer.SelectedPlacedDefaultTower;
            GameObject newObj = Instantiate(NewUpgrade.gameObject, lastDefaultTower.transform.position, lastDefaultTower.transform.rotation, lastDefaultTower.transform.parent);

            Destroy(TowerPlacer.SelectedPlacedDefaultTower.gameObject);

    //        TowerPlacer.SelectedTower = null;
            TowerPlacer.SelectedPlacedDefaultTower = newObj.GetComponent<DefaultTowerBehaviour>();
        }
    }
    public void SellTower()
    {
        MoneyManager.instance.AddMoney(Mathf.RoundToInt(TowerPlacer.SelectedPlacedDefaultTower.Config.purchaseCost* RefundRatio));
        Destroy(TowerPlacer.SelectedPlacedDefaultTower.gameObject);
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
        canvasGroup.alpha = TowerPlacer.SelectedPlacedDefaultTower == null ? 0 : 1;

        if(TowerPlacer.SelectedPlacedDefaultTower!=null)
        {
            TowerTitle.text = TowerPlacer.SelectedPlacedDefaultTower.Config.SelectedTitle;
            AnchorPos(TowerPlacer.SelectedPlacedDefaultTower.gameObject);
        }
    }
}
