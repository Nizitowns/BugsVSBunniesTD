using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BulletPool
    {
        public Queue<NewBulletBehaviour> _pooledObjects;

        public GameObject BulletPrefab;

        public BulletPool(GameObject bulletPrefab, int initSize)
        {
            _pooledObjects = new Queue<NewBulletBehaviour>();
            BulletPrefab = bulletPrefab;

            for (int i = 0; i < initSize; i++)
                AddObjectToPool();
        }

        private void AddObjectToPool()
        {
            var obj = MonoBehaviour.Instantiate(BulletPrefab, FolderHandler.Instance.BulletContainer);
            obj.SetActive(false);
            
            var behaviour = obj.GetComponent<NewBulletBehaviour>();
            behaviour.ConnectedPool = this;
            
            _pooledObjects.Enqueue(behaviour);
        }

        public NewBulletBehaviour GetBullet()
        {
            if (_pooledObjects.Count > 1)
            {
                var obj = _pooledObjects.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            AddObjectToPool();
            return GetBullet();
        }

        public void ReturnToPool(NewBulletBehaviour bulletBehaviour)
        {
            bulletBehaviour.gameObject.SetActive(false);
            _pooledObjects.Enqueue(bulletBehaviour);
        }
    }
}