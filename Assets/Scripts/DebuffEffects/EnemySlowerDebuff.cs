namespace DefaultNamespace.OnDeathEffects
{
    public class EnemySlowerDebuff : DebuffBase
    {
        public float speedReduction;

        private float speedBeforeDebuff = 0;
        private float timer = 0;

        public override void ApplyDebuff(Enemy enemy)
        {
            speedBeforeDebuff = enemy.agent.speed;
            enemy.agent.speed = speedBeforeDebuff * speedReduction;
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
            enemy.agent.speed = enemy.Config.speed;
        }

        public override void WhatHappensOnStack()
        {
            timer = 0;
        }
    }
}