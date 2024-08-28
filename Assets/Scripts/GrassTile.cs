using DefaultNamespace.TowerSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class GrassTile : MonoBehaviour
    {
        public TowerScriptableObject PlacedTower { get; private set; } = null;

        [SerializeField] private Transform PlacementPosition;
        
        public bool HasTowerOnIt => PlacedTower != null;

        public void AddTower(TowerScriptableObject towerScriptableObject)
        {
            if (PlacedTower) return;

            PlacedTower = towerScriptableObject;
            var ob = Instantiate(PlacedTower.prefab, PlacementPosition.position, Quaternion.identity);
            ob.GetComponent<NewTowerBase>().Config = PlacedTower;
            ob.AnimatedPlacement();
        }

        public void RemoveTower()
        {
            PlacedTower = null;
        }
    }
}