using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    public class SizeChangerDebuff : DebuffBase
    {
        public float sizeMultiplier;
        
        private float timer;
        
        public override void ApplyDebuff(Enemy enemy)
        {
            enemy.transform.DOScale(Vector3.one * sizeMultiplier, 0.5f);
        }

        public override void UpdateDebuff(Enemy enemy)
        {
            if (RunTimer(ref timer, duration, enemy))
            {
                WearOffDebuff(enemy);
            }
        }

        public override void WearOffDebuff(Enemy enemy)
        {
            IsWearOff = true;
            enemy.transform.DOScale(Vector3.one, 0.5f);
        }

        public override void WhatHappensOnStack()
        {
            timer = 0;
        }
    }
}