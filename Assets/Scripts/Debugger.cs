using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class Debugger : MonoBehaviour
    {
        public static Debugger Instance;

        public bool ShowPaths = false;
        public bool ShowObstacleFacingDirection = false;

        private void Awake()
        {
            if (Instance != null) Debug.LogError("There are Debuggers more than 1");
            
            Instance = this;
        }
    }
}