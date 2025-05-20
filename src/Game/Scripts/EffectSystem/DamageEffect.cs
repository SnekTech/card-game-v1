using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameV1.EffectSystem;

public class DamageEffect(int amount) : IEffect
{
    public async Task ExecuteAllAsync(IEnumerable<ITarget> targets)
    {
        var tasks = targets.Select(target => target.TakeDamageAsync(amount)).ToList();
        await Task.WhenAll(tasks);
    }
}