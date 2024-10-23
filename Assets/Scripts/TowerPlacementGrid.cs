using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(CoDissolve(true));
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
            
            StartCoroutine(CoDissolve(false));
            PlacedTowerObject.AnimatedRemove();
        }


        private IEnumerator CoDissolve(bool dissolveIn)
        {
            float elapsedTime = 0;
            var renderers = PlacedTowerObject.GetComponentsInChildren<Renderer>();
            List<Material> materials = new List<Material>();
            foreach (var render in renderers)
            {
                materials.Add(render.material);
            }
            
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                float dissolveAmount = 0;
                
                if (dissolveIn)
                    dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / 1);
                else
                    dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / 1);

                foreach (var material in materials)
                {
                    material.SetFloat("_DissolveStrenght", dissolveAmount);
                }
                yield return null;
            }
        }
    }
}