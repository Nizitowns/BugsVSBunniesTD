using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.Obstacle
{
    public class ObstaclePlacer : MonoBehaviour
    {
        public GameObject TestPrefab;
        public GameObject previewObject;

        private PathTileObject lastHitTileObject;

        private void Start()
        {
            previewObject = Instantiate(TestPrefab);
        }

        private void Update()
        {
            if (previewObject == null) return;
            
            previewObject.transform.position = InputGather.Instance.GetMousePosition();

            Debug.Log(CanPlacable());

            if (CanPlacable())
                if (InputGather.Instance.MouseLeftClick)
                    PlaceObstacle();

            if (InputGather.Instance.CancelButton)
                StopOstacleBuild();

        }
        
        private bool CanPlacable()
        {
            if (InputGather.isMouseOverGameObject) return false;

            lastHitTileObject = InputGather.Instance.GetHitTransform<PathTileObject>();
            
            if (lastHitTileObject == null) return false;

            return !lastHitTileObject.HasObstacle;
        }
        
        public void StartObstacleBuild(GameObject prefab)
        {
            
        }
        
        public void StopOstacleBuild()
        {
            Destroy(previewObject);
            previewObject = null;
        }

        private void PlaceObstacle()
        {
            // Destroy(previewObject);
            // previewObject = null;
            //
            // Debug.Log("Placed");

            lastHitTileObject.HasObstacle = true;
            var ob = Instantiate(TestPrefab, lastHitTileObject.transform.position + Vector3.up * 3, lastHitTileObject.directionData.GetRotation);
        }
    }
}