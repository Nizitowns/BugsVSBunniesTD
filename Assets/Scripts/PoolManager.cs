using System.Collections.Generic;
using DefaultNamespace.TowerSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        [SerializeField] private Transform BulletContainer;
        [SerializeField] private List<PoolID> pools;

        private void Awake()
        {
            Instance = this;
            pools = new List<PoolID>();
        }

        public void CreatePool(BulletConfig bulletConfig, int initSize)
        {
            foreach (var pool in pools)
            {
                if (pool.ID == bulletConfig.ID) return;
            }
            
            pools.Add(new PoolID(bulletConfig, initSize));
        }

        private void Update()
        {
            // Debug.Log(pools[0].ID + " Has - " + pools[0]._pooledObjects.Count);
        }

        public NewBulletBehaviour GetBullet(int ID)
        {
            foreach (var pool in pools)
            {
                if (pool.ID == ID)
                {
                    return pool.GetBullet();
                }
            }
            Debug.LogError("Bullet ID Could Not Found - Make Sure It's Added To The Pool");
            return null;
        }

        public class PoolID
        {
            public int ID;
            
            private GameObject BulletPrefab;
            public Queue<NewBulletBehaviour> _pooledObjects;
            
            public PoolID(BulletConfig bulletConfig, int initSize)
            {
                ID = bulletConfig.ID;
                BulletPrefab = bulletConfig.prefab;
                _pooledObjects = new Queue<NewBulletBehaviour>();

                for (int i = 0; i < initSize; i++)
                    AddObjectToPool();
            }

            private void AddObjectToPool()
            {
                var obj = Instantiate(BulletPrefab, Instance.BulletContainer);
                obj.SetActive(false);
            
                var behaviour = obj.GetComponent<NewBulletBehaviour>();
                behaviour.ConnectedPool = this;
            
                _pooledObjects.Enqueue(behaviour);
            }

            public NewBulletBehaviour GetBullet()
            {
                if (_pooledObjects.Count < 1)
                    AddObjectToPool();
                
                var obj = _pooledObjects.Dequeue();
                obj.Reset();
                obj.gameObject.SetActive(true);
                return obj;
            }

            public void ReturnToPool(NewBulletBehaviour bulletBehaviour)
            {
                bulletBehaviour.gameObject.SetActive(false);
                bulletBehaviour.transform.position = Vector3.zero;
                _pooledObjects.Enqueue(bulletBehaviour);
            }
        }
    }
}