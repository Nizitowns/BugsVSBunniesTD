using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    public abstract class DebuffBase : MonoBehaviour
    {
        public float duration = 0;
        protected float timer = 0;
        public bool IsWearOff { get; protected set; }
        public abstract void ApplyDebuff(IEnemyUnit enemy);
        /// <summary>
        /// Returns true on wear off
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public abstract bool UpdateDebuff(IEnemyUnit enemy, float tick);
        public abstract void WearOffDebuff(IEnemyUnit enemy);

        protected bool RunTimer(float tick)
        {
            timer += tick;
            if (timer > duration)
            {
                return true;
            }
            return false;
        }

        public virtual void WhatHappensOnStack(IEnemyUnit enemy, DebuffBase newDebuff)
        {
            duration = 0;
        }
    }
}