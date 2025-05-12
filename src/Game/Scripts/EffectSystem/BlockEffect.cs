using System.Collections.Generic;
using Godot;

namespace CardGameV1.EffectSystem;

public class BlockEffect(int amount) : IEffect
{
    public void Execute(IEnumerable<Node> targetNodes)
    {
        foreach (var node in targetNodes)
        {
            if (node is ITarget target)
            {
                target.Stats.Block += amount;
            }
        }
    }
}