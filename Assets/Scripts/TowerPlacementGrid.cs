using DefaultNamespace.TowerSystem;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class TowerPlacementGrid : MonoBehaviour
    {
        public TowerScriptableObject PlacedTowerConfig { get; private set; } = null;

        private GameObject PlacedTowerObject;
        [SerializeField] private Transform PlacementPosition;

        public Transform GetPlacementPosition => PlacementPosition;
        
        public bool HasTowerOnIt => PlacedTowerConfig != null;

        public void AddTower(TowerScriptableObject towerScriptableObject)
        {
            if (HasTowerOnIt) return;

            PlacedTowerConfig = towerScriptableObject;
            PlacedTowerObject = Instantiate(PlacedTowerConfig.prefab, PlacementPosition.position, Quaternion.identity);
            PlacedTowerObject.GetComponent<NewTowerBase>().Initiliaze(PlacedTowerConfig);
            PlacedTowerObject.AnimatedPlacement();
        }

        public void UpgradeTower(TowerScriptableObject towerScriptableObject)
        {
            if (!HasTowerOnIt) return;
            
            RemoveTower(true);
            AddTower(towerScriptableObject);
        }

        public void RemoveTower(bool immediate = false)
        {
            PlacedTowerConfig = null;
            PlacedTowerObject.GetComponent<NewTowerBase>().IsDisabled = true;

            if (immediate)
            {
                Destroy(PlacedTowerObject);
                return;
            }
            
            PlacedTowerObject.AnimatedRemove();
        }
    }
}