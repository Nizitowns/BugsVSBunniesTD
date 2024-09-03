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

        Tween trackedTween;
        public void AddTower(TowerScriptableObject towerScriptableObject)
        {
            if (HasTowerOnIt) return;

            PlacedTowerConfig = towerScriptableObject;
            PlacedTowerObject = Instantiate(PlacedTowerConfig.prefab, PlacementPosition.position, Quaternion.identity);
            PlacedTowerObject.GetComponent<NewTowerBase>().Initiliaze(PlacedTowerConfig);
            trackedTween?.Kill();
            trackedTween =PlacedTowerObject.AnimatedPlacement();
        }

        public void UpgradeTower(TowerScriptableObject towerScriptableObject)
        {
            if (!HasTowerOnIt) return;
            
            RemoveTower(true);
            AddTower(towerScriptableObject);
        }

        public void RemoveTower(bool immediate=false)
        {
            PlacedTowerConfig = null;
            PlacedTowerObject.GetComponent<NewTowerBase>().IsDisabled = true;
            //Destroy(PlacedTowerObject);
            if (immediate)
            {
                Destroy(PlacedTowerObject);
            }
            else
            {
                trackedTween?.Kill();
                trackedTween = PlacedTowerObject.AnimatedRemove().OnComplete(() =>
                 {
                     Destroy(PlacedTowerObject);
                 });
            }
        }
        private void OnDestroy()
        {
            trackedTween?.Kill();
        }
    }
}