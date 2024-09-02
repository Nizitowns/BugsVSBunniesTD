using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;

public interface IDebuffable
{
    public List<Debuff> ActiveDebuffs { get; }
    public void AddDebuff(Debuff newDebuff);
    public void HandleDebuff();
}