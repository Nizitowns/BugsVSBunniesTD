
using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    [CreateAssetMenu(menuName = "Debuffs/Slow Debuff")]
    public class EnemySlowerDebuff : Debuff
    {
        // Effect strength can be between 0 and 1. 0.2 means %20 reduction.
        public override void ApplyEffect(IEnemyUnit enemy)
        {
            enemy.Speed *= 1 - effectStrength;
        }

        public override void UpdateDebuff(float deltaTime)
        {
            
        }

        public override void RemoveEffect(IEnemyUnit enemy)
        {
            enemy.Speed /= 1 - effectStrength;
        }
    }
}