using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.TowerSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PurchaseButton : UIButtonBase
{
    public TowerScriptableObject TowerScriptable;
    [HideInInspector]
    AudioSource source;
    private Button button;
    private TextMeshProUGUI cost;
    private Image icon;
    private Image bg;

    public override void OnAwake()
    {
        source = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        bg= GetComponent<Image>();
        cost = GetComponentInChildren<TextMeshProUGUI>();
        icon = transform.GetChild(0).GetComponent<Image>();
    }

    public override void OnStart()
    {
        UpdateIcon();
    }


    public void UpdateIcon()
    {
        button.interactable = MoneyManager.instance.Balance >= TowerScriptable.purchaseCost && TowerScriptable.prefab!=null;
        if(!button.interactable&& (TowerPlacer.SelectedTower == this))
        {
            TowerPlacer.SelectedTower = null;
        }
        cost.text = TowerScriptable.purchaseCost + "$";
        icon.sprite = TowerScriptable.purchaseIcon;
        icon.color = button.targetGraphic.canvasRenderer.GetColor();
        if(!button.interactable&&EventSystem.current.currentSelectedGameObject==this.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        if (TowerPlacer.SelectedTower == this)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public override void OnToggle(bool toggle)
    {
        if(source!=null&& source.enabled) PlaySoundFX();

        if (toggle)
            TowerPlacer.SelectedTower = this;
        else
            TowerPlacer.SelectedTower = null;
    }


    public override void PlaySoundFX()
    {
        source.Play();
    }

    public override void OnMouseEnter()
    {
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
    }

    public override void OnUpdate()
    {
        UpdateIcon();

    }
}
