using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardGameV1.EffectSystem;

public interface IEffect
{
    Task ExecuteAllAsync(IEnumerable<ITarget> targets);
}