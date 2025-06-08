using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.CustomResources.Cards.Warrior;

[GlobalClass]
public partial class WarriorSlash : Card
{
    private const int DamageAmount = 6;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorSlash),
        Cost = 2,
        Type = CardType.Attack,
        Rarity = CardRarity.Common,
        Target = CardTarget.AllEnemies,
        TooltipText = $"[center]Deal [color=\"ff0000\"]{DamageAmount}[/color] damage to all enemies.[/center]",
        IconPath = "res://art/tile_0118.png",
        SoundPath = "res://art/slash.ogg"
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var damageEffect = new DamageEffect(DamageAmount)
        {
            Sound = Sound
        };
        await damageEffect.ExecuteAllAsync(targets);
    }
}