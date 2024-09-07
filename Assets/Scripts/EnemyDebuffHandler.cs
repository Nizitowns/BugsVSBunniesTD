using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;
using UnityEngine;

public class EnemyDebuffHandler : IDebuffHandler
{
    public List<Debuff> ActiveDebuffs { get; } = new List<Debuff>();
    private IEnemyUnit unit;

    public EnemyDebuffHandler(IEnemyUnit unit)
    {
        this.unit = unit;
    }

    public void ApplyDebuff(Debuff newDebuff)
    {
        var existingDebuff = ActiveDebuffs.Find(d => d.GetType() == newDebuff.GetType());

        if (existingDebuff != null)
        {
            if (existingDebuff.isStackable)
                existingDebuff.Stack(newDebuff.duration, newDebuff.effectStrength);
            else
                existingDebuff.DefaultStack(newDebuff.duration, newDebuff.effectStrength);
            
            existingDebuff.RemoveEffect(unit);
            existingDebuff.ApplyEffect(unit);
            return;
        }

        Debuff debuffInstance = MonoBehaviour.Instantiate(newDebuff);
        debuffInstance.Initialize(newDebuff.effectStrength, newDebuff.duration);
        debuffInstance.ApplyEffect(unit);

        ActiveDebuffs.Add(debuffInstance);
    }

    public void HandleDebuff()
    {
        for (int i = ActiveDebuffs.Count - 1; i >= 0; i--)
        {
            ActiveDebuffs[i].UpdateDebuff(Time.deltaTime);
            
            if (ActiveDebuffs[i].UpdateTimer(Time.deltaTime))
            {
                ActiveDebuffs[i].RemoveEffect(unit);
                ActiveDebuffs.RemoveAt(i);
            }
        }
    }
}