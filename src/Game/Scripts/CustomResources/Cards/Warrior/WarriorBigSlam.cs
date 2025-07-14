using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;

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
        TooltipText = GenerateTooltipText(BaseDamageAmount, ExposedDuration),
        IconPath = "res://art/tile_0117.png",
        SoundPath = "res://art/slash.ogg",
    };

    private const int BaseDamageAmount = 4;
    private const int ExposedDuration = 2;

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

        var statusEffect = new AddStatusEffect(StatusFactory.CreateExposed(ExposedDuration));
        await statusEffect.ExecuteAllAsync(targetList, cancellationToken);
    }

    private static string GenerateTooltipText(int damage, int exposedDuration) =>
        $"[center]Deal [color=\"ff0000\"]{damage}[/color] damage and apply {exposedDuration} [color=\"ffdf00\"] Exposed[/color].[/center]";

    public override string GetUpdatedTooltipText(ModifierHandler playerModifierHandler,
        ModifierHandler? enemyModifierHandler = null)
    {
        var modifiedDamage = playerModifierHandler.GetModifiedValue(BaseDamageAmount, ModifierType.DamageDealt);
        if (enemyModifierHandler != null)
        {
            modifiedDamage = enemyModifierHandler.GetModifiedValue(modifiedDamage, ModifierType.DamageTaken);
        }

        return GenerateTooltipText(modifiedDamage, ExposedDuration);
    }
}