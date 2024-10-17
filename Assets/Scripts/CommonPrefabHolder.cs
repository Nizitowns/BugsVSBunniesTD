using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CommonPrefabHolder : MonoBehaviour
    {
        public static CommonPrefabHolder Instance;
        
        public GameObject BulletAreaVisualizer;

        public GameObject HealtBarPrefab;

        private void Awake()
        {
            Instance = this;
        }
    }
}