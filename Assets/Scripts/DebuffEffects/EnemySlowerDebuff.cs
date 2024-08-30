namespace DefaultNamespace.OnDeathEffects
{
    public class EnemySlowerDebuff : DebuffBase
    {
        public float speedReduction;

        private float speedBeforeDebuff = 0;

        public override void ApplyDebuff(IEnemyUnit enemy)
        {
            speedBeforeDebuff = enemy.Speed;
            enemy.Speed = speedBeforeDebuff * speedReduction;
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
            enemy.Speed = enemy.Config.speed;
        }

        public override void WhatHappensOnStack(IEnemyUnit enemy, DebuffBase newDebuff)
        {
            if (newDebuff is EnemySlowerDebuff debuff)
            {
                if (debuff.speedReduction < speedReduction)
                {
                    WearOffDebuff(enemy);
                    debuff.ApplyDebuff(enemy);
                }
            }
                
            timer = 0;
        }
    }
}