using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    public abstract class DebuffBase : MonoBehaviour
    {
        public float duration = 0;
        public bool IsWearOff { get; protected set; }
        public abstract void ApplyDebuff(Enemy enemy);
        public abstract void UpdateDebuff(Enemy enemy);
        public abstract void WearOffDebuff(Enemy enemy);

        protected bool RunTimer(ref float passedTime, float duration, Enemy enemy)
        {
            passedTime += Time.deltaTime;
            if (passedTime > duration)
            {
                return true;
            }

            return false;
        }

        public abstract void WhatHappensOnStack();
    }
}