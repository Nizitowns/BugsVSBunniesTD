using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        private Transform camTransform;
        private Transform _targetTransform;
        private Slider _slider;

        private void Awake()
        {
            camTransform = Camera.main.transform;
            _slider = GetComponentInChildren<Slider>();
        }

        public void Init(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        private void Update()
        {
            transform.LookAt(camTransform);
            transform.position = _targetTransform.position + Vector3.up * 10;
        }

        public void UpdateHeahtBar(float currentHP, float maxHP)
        {
            _slider.value = currentHP / maxHP;
        }
    }
}