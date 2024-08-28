using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.TowerSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PurchaseButton : MonoBehaviour
{
    public TowerScriptableObject TowerScriptable;
    [HideInInspector]
    AudioSource source;
    private Button button;
    private TextMeshProUGUI cost;
    private Image icon;
    private Image bg;
    void Start()
    {
        source = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        bg= GetComponent<Image>();
        cost = GetComponentInChildren<TextMeshProUGUI>();
        icon = transform.GetChild(0).GetComponent<Image>();

    }
    public void UpdateIcon()
    {
        button.interactable = MoneyManager.instance.Balance >= TowerScriptable.purchaseCost && TowerScriptable.prefab!=null;
        if(!button.interactable&& (TowerPlacer.SelectedTower == this))
        {
            TowerPlacer.SelectedTower = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
        cost.text = TowerScriptable.purchaseCost + "$";
        icon.sprite = TowerScriptable.purchaseIcon;
        icon.color = button.targetGraphic.canvasRenderer.GetColor();
        if (TowerPlacer.SelectedTower == this)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void ToggleSelected()
    {
        if(source!=null&& source.enabled)
        {
            source.Play();
        }
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
