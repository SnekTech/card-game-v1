using System.Collections.Generic;

namespace CardGameV1.EffectSystem;

public interface IEffect
{
    void Execute(IEnumerable<ITarget> targets);
}