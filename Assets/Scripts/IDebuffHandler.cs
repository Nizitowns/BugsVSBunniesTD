using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;

public interface IDebuffHandler
{
    public List<Debuff> ActiveDebuffs { get; }
    public void ApplyDebuff(Debuff newDebuff);
    public void HandleDebuff();
}

public interface IDebuffable
{
    public void ApplyDebuff(Debuff newDebuff);
}