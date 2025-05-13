using System.Collections.Generic;

namespace CardGameV1.EffectSystem;

public class BlockEffect(int amount) : IEffect
{
    public void Execute(IEnumerable<ITarget> targets)
    {
        foreach (var target in targets)
        {
            target.Stats.Block += amount;
        }
    }
}