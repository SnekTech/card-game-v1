using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.CustomResources.Cards.Warrior;

[GlobalClass]
public partial class WarriorBigSlam : Card
{
    private const int DamageAmount = 10;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorBigSlam),
        Cost = 2,
        Type = CardType.Attack,
        Rarity = CardRarity.Uncommon,
        Target = CardTarget.SingleEnemy,
        TooltipText = $"[center]Deal [color=\"ff0000\"]{DamageAmount}[/color] damage and apply 2 [color=\"ffdf00\"] Exposed[/color].[/center]",
        IconPath = "res://art/tile_0117.png",
        SoundPath = "res://art/slash.ogg"
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var damageEffect = new DamageEffect(DamageAmount)
        {
            Sound = Sound
        };
        
        // todo: apply Exposed status
        GD.Print("apply exposed");
        
        await damageEffect.ExecuteAllAsync(targets);
    }
}