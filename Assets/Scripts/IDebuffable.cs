using System.Collections.Generic;
using DefaultNamespace.OnDeathEffects;

public interface IDebuffable
{
    public List<DebuffBase> WearDebuffs { get; }
    public void AddDebuff(DebuffBase newDebuff);
    public void HandleDebuff();
}