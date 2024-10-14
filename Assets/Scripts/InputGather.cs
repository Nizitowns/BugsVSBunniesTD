using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class InputGather : MonoBehaviour
    {
        public static InputGather Instance;

        private Camera mainCam;
        [SerializeField] private LayerMask mouseOverLayers;
        [Tooltip("Custom Input Smoothness Sensitivity")]
        [SerializeField] private float sensitivity = 10;

        public float AxisX { get; private set; }
        public float AxisY { get; private set; }
        public float AxisRawX { get; private set; }
        public float AxisRawY { get; private set; }
        
        public float OrbitRaw { get; private set; }
        public float ScroolWheel { get; private set; }
        public bool MouseLeftClick { get; private set; }
        public bool XClick { get; private set; }
        public bool SpaceButton { get; private set; }
        public bool CancelButton { get; private set; }
        
        private Vector2 _customAxis = Vector2.zero;
        private float _customOrbit = 0;

        public Vector2 Axis => new Vector2(AxisX, AxisY);
        public Vector2 AxisRaw => new Vector2(AxisRawX, AxisRawY);
        
        /// <summary>
        /// Returns Normalized Frame Independent Custom Axis
        /// </summary>
        public Vector2 AxisNormalized
        {
            get
            {
                if (_customAxis.magnitude > 1)
                {
                    return _customAxis.normalized;
                }
                return _customAxis;
            }
        }

        /// <summary>
        /// Returns Frame Independent Orbit Value
        /// </summary>
        public float Orbit => _customOrbit;

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
            AxisX = Input.GetAxis("Vertical");
            AxisY = Input.GetAxis("Horizontal");
            
            AxisRawX = Input.GetAxisRaw("Vertical");
            AxisRawY = Input.GetAxisRaw("Horizontal");
            
            OrbitRaw = Input.GetAxisRaw("Orbit");
            ScroolWheel = Input.GetAxis("Mouse ScrollWheel");

            MouseLeftClick = Input.GetMouseButtonDown(0);
            XClick = Input.GetKeyDown(KeyCode.X);
            SpaceButton = Input.GetKeyDown(KeyCode.Space);
            CancelButton = Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1);
            
            UpdateCustomInputs();
        }

        private void UpdateCustomInputs()
        {
            // Frame Independent Axis value
            if (AxisRaw != Vector2.zero)
                _customAxis = Vector2.Lerp(_customAxis, AxisRaw, sensitivity * Time.unscaledDeltaTime);
            else
                _customAxis = Vector2.Lerp(_customAxis, Vector2.zero, sensitivity * Time.unscaledDeltaTime);
            
            // Frame Independent Orbit value
            if (OrbitRaw != 0)
            {
                if(OrbitRaw > 0)
                    _customOrbit = Mathf.Lerp(_customOrbit, 1, sensitivity * Time.unscaledDeltaTime);
                else
                    _customOrbit = Mathf.Lerp(_customOrbit, -1, sensitivity * Time.unscaledDeltaTime);
            }
            else
            {
                _customOrbit = Mathf.Lerp(_customOrbit, 0, sensitivity * Time.unscaledDeltaTime);
            }
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