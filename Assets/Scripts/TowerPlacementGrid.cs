using DefaultNamespace.TowerSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class TowerPlacementGrid : MonoBehaviour
    {
        public TowerScriptableObject PlacedTowerConfig { get; private set; } = null;

        private GameObject PlacedTowerObject;
        [SerializeField] private Transform PlacementPosition;
        
        public bool HasTowerOnIt => PlacedTowerConfig != null;

        public void AddTower(TowerScriptableObject towerScriptableObject)
        {
            if (PlacedTowerConfig) return;

            PlacedTowerConfig = towerScriptableObject;
            PlacedTowerObject = Instantiate(PlacedTowerConfig.prefab, PlacementPosition.position, Quaternion.identity);
            PlacedTowerObject.GetComponent<NewTowerBase>().Config = PlacedTowerConfig;
            PlacedTowerObject.AnimatedPlacement();
        }

        public void UpgradeTower(TowerScriptableObject towerScriptableObject)
        {
            if (!PlacedTowerConfig) return;
            
            RemoveTower();
            AddTower(towerScriptableObject);
        }

        public void RemoveTower()
        {
            PlacedTowerConfig = null;
            PlacedTowerObject.AnimatedRemove();
            Destroy(PlacedTowerObject, 1);
            PlacedTowerObject = null;
        }
    }
}