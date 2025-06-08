using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.CustomResources.Cards.Warrior;

[GlobalClass]
public partial class WarriorAxeAttack : Card
{
    private const int DamageAmount = 6;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorAxeAttack),
        Cost = 1,
        Type = CardType.Attack,
        Rarity = CardRarity.Common,
        Target = CardTarget.SingleEnemy,
        TooltipText = $"[center]Deal [color=\"ff0000\"]{DamageAmount}[/color] damage.[/center]",
        IconPath = "res://art/tile_0119.png",
        SoundPath = "res://art/axe.ogg"
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