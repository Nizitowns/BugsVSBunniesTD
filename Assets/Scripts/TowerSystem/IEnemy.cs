using UnityEngine;

namespace DefaultNamespace.TowerSystem
{
    public interface IEnemy
    {
        public bool isDead { get; }
        public Transform mTransform { get; }
    }
}