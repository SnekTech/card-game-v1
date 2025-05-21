using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.Character.Cards;

[GlobalClass]
public partial class WarriorBlock : Card
{
    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var blockEffect = new BlockEffect(5)
        {
            Sound = Sound
        };
        await blockEffect.ExecuteAllAsync(targets);
    }
}