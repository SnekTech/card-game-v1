using System.Collections.Generic;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.Character.Cards;

[GlobalClass]
public partial class WarriorBlock : Card
{
    protected override void ApplyEffects(IEnumerable<ITarget> targets)
    {
        var blockEffect = new BlockEffect(5);
        blockEffect.Execute(targets);
    }
}