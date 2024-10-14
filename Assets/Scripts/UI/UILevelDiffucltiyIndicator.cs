using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UILevelDiffucltiyIndicator : MonoBehaviour
    {
        [SerializeField] private Sprite easy, normal, boss;

        private Image _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Image>();
        }

        private void OnEnable()
        {
            EnemySpawner.OnDiffucltyChanged += UpdateIcon;
        }

        private void OnDisable()
        {
            EnemySpawner.OnDiffucltyChanged -= UpdateIcon;
        }

        private void UpdateIcon(EnemySpawner.EnemyWave.DifficultyRating difficultyRating)
        {
            switch (difficultyRating)
            {
                case EnemySpawner.EnemyWave.DifficultyRating.Normal:
                    _renderer.sprite = normal;
                    break;
                case EnemySpawner.EnemyWave.DifficultyRating.Easy:
                    _renderer.sprite = easy;
                    break;
                case EnemySpawner.EnemyWave.DifficultyRating.Boss:
                    _renderer.sprite = boss;
                    break;
                default:
                    _renderer.sprite = null;
                    break;  
            }
        }
    }
}