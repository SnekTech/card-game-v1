using System.Threading;
using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
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
        TooltipText = GenerateTooltipText(MusclePerTurn),
        IconPath = "res://art/tile_0127.png",
        SoundPath = "res://art/true_strength.ogg",
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler _,
        CancellationToken cancellationToken)
    {
        var trueStrengthStatus = StatusFactory.Create<TrueStrengthForm>();
        var addTrueStrengthEffect = new AddStatusEffect(trueStrengthStatus);
        await addTrueStrengthEffect.ExecuteAllAsync(targets, cancellationToken);
    }

    private static string GenerateTooltipText(int musclePerTurn) =>
        $"[center]At the start of your turn, gain {musclePerTurn} [color=\"ffdf00\"] Muscle[/color].[/center]";

    public override string GetUpdatedTooltipText(ModifierHandler playerModifierHandler,
        ModifierHandler? enemyModifierHandler = null) => GenerateTooltipText(MusclePerTurn);
}