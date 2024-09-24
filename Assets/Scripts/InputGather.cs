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

        [HideInInspector] public Vector2 Axis;
        [HideInInspector] public Vector2 AxisRaw;
        [HideInInspector] public float ScroolWheel;
        [HideInInspector] public bool MouseLeftClick;
        [HideInInspector] public bool XClick;
        [HideInInspector] public bool CancelButton;
        
        public Vector2 AxisNormalized
        {
            get
            {
                if (Axis.magnitude > 1) return Axis.normalized;
                return Axis;
            }
        }

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
            Axis.x = Input.GetAxis("Vertical");
            Axis.y = Input.GetAxis("Horizontal");
            
            AxisRaw.x = Input.GetAxisRaw("Vertical");
            AxisRaw.y = Input.GetAxisRaw("Horizontal");
            ScroolWheel = Input.GetAxis("Mouse ScrollWheel");
            
            MouseLeftClick = Input.GetMouseButtonDown(0);
            XClick = Input.GetKey(KeyCode.X);
            CancelButton = Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1);
        }

        public Vector3 GetMousePosition()
        {
            RaycastHit hit;
            float maxDistance = 9999;

            if (Physics.Raycast(DefaulttRay(), out hit, maxDistance, mouseOverLayers))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
        
        public T GetHitClass<T>() where T : class
        {
            float maxDistance = 9999;

            var hits = Physics.RaycastAll(DefaulttRay(), maxDistance);

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out T t))
                {
                    return t;
                }
            }

            return null;
        }

        private Ray DefaulttRay()
        {
            var mousePOs = Input.mousePosition;
            mousePOs.z = mainCam.nearClipPlane;
            return mainCam.ScreenPointToRay(mousePOs);
        }
    }
}