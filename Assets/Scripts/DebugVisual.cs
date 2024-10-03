using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DebugVisual : MonoBehaviour
    {
        public static DebugVisual Instance;
        
        public GameObject BulletAreaVisualizer;

        private void Awake()
        {
            Instance = this;
        }
    }
}