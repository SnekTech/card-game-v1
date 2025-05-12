using System.Collections.Generic;
using Godot;

namespace CardGameV1.EffectSystem;

public interface IEffect
{
    void Execute(IEnumerable<Node> targetNodes);
}