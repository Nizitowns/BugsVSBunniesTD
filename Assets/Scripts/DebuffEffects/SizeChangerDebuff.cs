
using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    [CreateAssetMenu(menuName = "Debuffs/SizeChange Debuff")]
    public class SizeChangerDebuff : Debuff
    {
        public override void ApplyEffect(IEnemyUnit enemy)
        {
        }

        public override void UpdateDebuff(float deltaTime)
        {
            
        }

        public override void RemoveEffect(IEnemyUnit enemy)
        {
        }
    }
}