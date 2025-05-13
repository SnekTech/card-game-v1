using System.Collections.Generic;

namespace CardGameV1.EffectSystem;

public class DamageEffect(int amount) : IEffect
{
    public void Execute(IEnumerable<ITarget> targets)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(amount);
        }
    }
}