using System;
using System.Security.Cryptography;
using DefaultNamespace;
using DefaultNamespace.TowerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    public float RefundRatio = 0.75f;
    CanvasGroup canvasGroup;
    AudioSource source;
    void Start()
    {
    source = GetComponent<AudioSource>();
        canvasGroup=GetComponent<CanvasGroup>();
        canvas=transform.parent.GetComponent<Canvas>();
    }
    Canvas canvas;
    public TextMeshProUGUI TowerTitle;

    /*
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
    */
    public UpgradeButton Upgrade1;
    public UpgradeButton Upgrade2;

    private void UpdateButtons(bool reset)
    {
        if (reset)
        {
            Upgrade1.Reset();
            Upgrade2.Reset();
            return;
        }
        
        if (TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions.Length > 0)
            Upgrade1.UpdateConfig(TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[0]);
        else
            Upgrade1.Reset();
        
        if (TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions.Length > 1)
            Upgrade2.UpdateConfig(TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[1]);
        else
            Upgrade2.Reset();
    }

    /*
    public void CheckButtons()
    {
        if (TowerPlacer.TowerPlacementGrid!=null)
        {
            if (TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions.Length > 0)
            {
                Upgrade1.ResetGraphics();
                
                Upgrade1.Icon.color = Color.Lerp(TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[0].purchaseIconAdditionalTint, Upgrade1.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade1.Icon.enabled = true;
                Upgrade1.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[0].purchaseCost;
                Upgrade1.Icon.sprite = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[0].purchaseIcon;
                Upgrade1.Cost.text = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[0].purchaseCost + "$";
            }
            else
            {
                Upgrade1.ResetAll();
            }
            if (TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions.Length > 1)
            {
                Upgrade2.ResetGraphics();
                
                
                Upgrade2.Icon.color =Color.Lerp(TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[1].purchaseIconAdditionalTint,Upgrade2.Btn.targetGraphic.canvasRenderer.GetColor(),0.5f);
                Upgrade2.Icon.enabled = true;
                Upgrade2.Icon.sprite = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[1].purchaseIcon;
                Upgrade2.Btn.interactable = MoneyManager.instance.Balance >= TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[1].purchaseCost;
                Upgrade2.Cost.text = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[1].purchaseCost + "$";
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
    */

    public void UpgradeTower(int upgradeSlot)
    {
        TowerScriptableObject NewUpgrade = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.upgradeOptions[upgradeSlot];
        if (MoneyManager.instance.RemoveMoney(NewUpgrade.purchaseCost))
        {
            if(source!=null)
            {
                source.Play();
            }
            TowerPlacer.TowerPlacementGrid.UpgradeTower(NewUpgrade);
            TowerPlacer.TowerPlacementGrid = null;
        }
    }
    public void SellTower()
    {
        MoneyManager.instance.AddMoney(Mathf.RoundToInt(TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.purchaseCost* RefundRatio));
        
        TowerPlacer.TowerPlacementGrid.RemoveTower();
        TowerPlacer.TowerPlacementGrid = null;

        // Destroy(TowerPlacer.SelectedPlacedDefaultTower.gameObject);
    }

    //Code Ripped Straight From the Internet :3
    public void AnchorPos(GameObject targetObject)
    {
        if (targetObject == null)
            return;

        // Step 1: Get the screen position of the target object
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetObject.transform.position + Vector3.up * 10);

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
        canvasGroup.alpha = TowerPlacer.TowerPlacementGrid == null ? 0 : 1;

        if(TowerPlacer.TowerPlacementGrid!=null)
        {
            UpdateButtons(false);
            TowerTitle.text = TowerPlacer.TowerPlacementGrid.PlacedTowerConfig.selectedTitle;
            AnchorPos(TowerPlacer.TowerPlacementGrid.gameObject);
        }
        else
        {
            UpdateButtons(true);
        }
    }
}
