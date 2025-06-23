using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using CardGameV1.StatusSystem;
using CardGameV1.StatusSystem.BuiltinStatuses;

namespace CardGameV1.CustomResources.Cards.Warrior;

public class WarriorTrueStrength : Card
{
    private const int MusclePerTurn = 2;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorTrueStrength),
        Cost = 3,
        Type = CardType.Power,
        Rarity = CardRarity.Rare,
        Target = CardTarget.Self,
        TooltipText =
            $"[center]At the start of your turn, gain {MusclePerTurn} [color=\"ffdf00\"] Muscle[/color].[/center]",
        IconPath = "res://art/tile_0127.png",
        SoundPath = "res://art/true_strength.ogg",
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets)
    {
        var trueStrengthStatus = StatusFactory.Create<TrueStrengthForm>();
        var addTrueStrengthEffect = new AddStatusEffect(trueStrengthStatus);
        await addTrueStrengthEffect.ExecuteAllAsync(targets);
    }
}