using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class InputGather : MonoBehaviour
    {
        public static InputGather Instance;

        private Camera mainCam;
        [SerializeField] private LayerMask mouseOverLayers;
        
        [HideInInspector] public bool MouseLeftClick;
        [HideInInspector] public bool CancelButton;
        
        public static bool isMouseOverGameObject => EventSystem.current.IsPointerOverGameObject();

        private void Awake()
        {
            if(Instance != null) Debug.LogError("There are InputGaters more than 1");
            Instance = this;
        }

        private void Start()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            MouseLeftClick = Input.GetMouseButtonDown(0);
            CancelButton = Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1);
        }

        public Vector3 GetMousePosition()
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            RaycastHit hit;
            float maxDistance = 9999;

            if (Physics.Raycast(ray, out hit, maxDistance, mouseOverLayers))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
        
        public T GetHitTransform<T>() where T : class
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            var ray = mainCam.ScreenPointToRay(mousePOs);

            float maxDistance = 9999;

            var hits = Physics.RaycastAll(ray, maxDistance);

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out T t))
                {
                    return t;
                }
            }

            return null;
        }
    }
}