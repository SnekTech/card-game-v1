using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardGameV1.EffectSystem;

public class BlockEffect(int amount) : IEffect
{
    public Task ExecuteAllAsync(IEnumerable<ITarget> targets)
    {
        foreach (var target in targets)
        {
            target.Stats.Block += amount;
        }

        return Task.CompletedTask;
    }
}