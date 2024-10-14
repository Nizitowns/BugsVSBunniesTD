using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FolderHandler : MonoBehaviour
    {
        public static FolderHandler Instance;
        
        public Transform BulletContainer;

        private void Awake()
        {
            Instance = this;
        }
    }
}