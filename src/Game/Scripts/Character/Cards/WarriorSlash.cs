using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.Character.Cards;

[GlobalClass]
public partial class WarriorSlash : Card
{
    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var damageEffect = new DamageEffect(4)
        {
            Sound = Sound
        };
        await damageEffect.ExecuteAllAsync(targets);
    }
}