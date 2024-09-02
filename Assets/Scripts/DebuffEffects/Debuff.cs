using UnityEngine;

namespace DefaultNamespace.OnDeathEffects
{
    public abstract class Debuff : ScriptableObject
    {
        public string debuffName;
        public Sprite icon;
        public float duration;
        public float effectStrength;
        public bool isStackable;

        protected float remainingDuration;

        public virtual void Initialize(float effectStrength, float duration)
        {
            this.effectStrength = effectStrength;
            this.duration = duration;
            remainingDuration = duration;
        }

        public void DefaultStack(float duration, float effectStrength)
        {
            if (this.duration < duration)
                remainingDuration = this.duration;
            else
                remainingDuration = duration;
            
            if (this.effectStrength < effectStrength) this.effectStrength = effectStrength;
        }

        public virtual void Stack(float duratin, float effectStrenght)
        {
            if (!isStackable) return;
        }

        public abstract void ApplyEffect(IEnemyUnit enemy);
        public abstract void UpdateDebuff(float deltaTime);
        public abstract void RemoveEffect(IEnemyUnit enemy);

        public bool UpdateTimer(float deltaTime)
        {
            remainingDuration -= deltaTime;
            return remainingDuration <= 0;
        }
    }
}