using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using CardGameV1.StatusSystem;
using CardGameV1.StatusSystem.BuiltinStatuses;
using Godot;

namespace CardGameV1.CustomResources.Cards.Warrior;

[GlobalClass]
public partial class WarriorBigSlam : Card
{
    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorBigSlam),
        Cost = 2,
        Type = CardType.Attack,
        Rarity = CardRarity.Uncommon,
        Target = CardTarget.SingleEnemy,
        TooltipText =
            $"[center]Deal [color=\"ff0000\"]{BaseDamage}[/color] damage and apply 2 [color=\"ffdf00\"] Exposed[/color].[/center]",
        IconPath = "res://art/tile_0117.png",
        SoundPath = "res://art/slash.ogg",
    };

    private const int BaseDamage = 4;
    private const int Duration = 2;

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var targetList = targets.ToList();
        var damageEffect = new DamageEffect(BaseDamage)
        {
            Sound = Sound,
        };

        await damageEffect.ExecuteAllAsync(targetList);

        var exposedStatus = StatusFactory.Create<Exposed>();
        exposedStatus.Duration = Duration;
        var statusEffect = new AddStatusEffect(exposedStatus);
        await statusEffect.ExecuteAllAsync(targetList);
    }
}