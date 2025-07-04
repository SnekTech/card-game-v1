﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;
using CardGameV1.StatusSystem.BuiltinStatuses;

namespace CardGameV1.CustomResources.Cards.Warrior;

public class WarriorBigSlam : Card
{
    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorBigSlam),
        Cost = 2,
        Type = CardType.Attack,
        Rarity = CardRarity.Uncommon,
        Target = CardTarget.SingleEnemy,
        ShouldExhaust = true,
        TooltipText =
            $"[center]Deal [color=\"ff0000\"]{BaseDamageAmount}[/color] damage and apply 2 [color=\"ffdf00\"] Exposed[/color].[/center]",
        IconPath = "res://art/tile_0117.png",
        SoundPath = "res://art/slash.ogg",
    };

    private const int BaseDamageAmount = 4;
    private const int Duration = 2;

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler modifierHandler,
        CancellationToken cancellationToken)
    {
        var targetList = targets.ToList();
        var modifiedValue = modifierHandler.GetModifiedValue(BaseDamageAmount, ModifierType.DamageDealt);
        var damageEffect = new DamageEffect(modifiedValue)
        {
            Sound = Sound,
        };

        await damageEffect.ExecuteAllAsync(targetList, cancellationToken);

        var exposedStatus = StatusFactory.Create<Exposed>();
        exposedStatus.Duration = Duration;
        var statusEffect = new AddStatusEffect(exposedStatus);
        await statusEffect.ExecuteAllAsync(targetList, cancellationToken);
    }
}