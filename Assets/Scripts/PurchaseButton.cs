using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class PurchaseButton : MonoBehaviour
{
    public GameObject TowerPrefab;
    public int PurchaseCost=1;

    private Button button;
    private TextMeshProUGUI cost;
    private Image icon;
    private Image bg;
    void Start()
    {
        button= GetComponent<Button>();
        bg= GetComponent<Image>();
        cost = GetComponentInChildren<TextMeshProUGUI>();
        icon = transform.GetChild(0).GetComponent<Image>();

    }
    public void UpdateIcon()
    {
        
        button.interactable = MoneyManager.instance.Balance >= PurchaseCost;
        if(!button.interactable&& (TowerPlacer.SelectedTower == this))
        {
            TowerPlacer.SelectedTower = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
        cost.text = PurchaseCost + "$";
        icon.color = button.targetGraphic.canvasRenderer.GetColor();
        if (TowerPlacer.SelectedTower == this)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ToggleSelected()
    {
        if (TowerPlacer.SelectedTower == this)
        {
            TowerPlacer.SelectedTower = null;
            EventSystem.current.SetSelectedGameObject(null);

        }
        else
        {
            TowerPlacer.SelectedTower = this;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    void Update()
    {
        UpdateIcon();
    }
}
