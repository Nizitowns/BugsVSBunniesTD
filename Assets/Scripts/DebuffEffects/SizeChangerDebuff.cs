using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    public class SizeChangerDebuff : DebuffBase
    {
        public float sizeMultiplier;
        
        private float timer;
        
        public override void ApplyDebuff(IEnemyUnit enemy)
        {
            enemy.mTransform.DOScale(Vector3.one * sizeMultiplier, 0.5f);
        }

        public override bool UpdateDebuff(IEnemyUnit enemy, float tick)
        {
            if (RunTimer(tick))
            {
                WearOffDebuff(enemy);
                return true;
            }
            return false;
        }

        public override void WearOffDebuff(IEnemyUnit enemy)
        {
            IsWearOff = true;
            enemy.mTransform.DOScale(Vector3.one, 0.5f);
        }
    }
}