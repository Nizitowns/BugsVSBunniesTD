using DefaultNamespace.TowerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UpgradeButton : UIButtonBase
    {
        private TowerScriptableObject _towerConfig;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI Cost;
        [SerializeField] private UIUpgradeHoverInfo _upgradeHoverInfo;

        public void UpdateConfig(TowerScriptableObject Config)
        {
            _towerConfig = Config;
            _icon.sprite = Config.purchaseIcon;
            _icon.enabled = true;
            _icon.color = Color.white;
            Cost.text = Config.purchaseCost.ToString();
            
            if(MoneyManager.instance.Balance < Config.purchaseCost)
                Interactable = false;
            else
                Interactable = true;
        }

        public override void OnClick()
        {
            OnHoverInfo(false);
        }

        public void Reset()
        {
            _icon.enabled = false;
            Cost.text = "---";
            Interactable = false;
        }
        
        public override void OnHoverInfo(bool toggle)
        {
            if (!Interactable) return;
            _upgradeHoverInfo.ToggleHoverInfo(toggle, _towerConfig.selectedTitle, _towerConfig.description);
        }
    }
}