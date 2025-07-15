using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;

namespace CardGameV1.CustomResources.Cards.Warrior;

public class WarriorSlash : Card
{
    private const int BaseDamageAmount = 6;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorSlash),
        Cost = 2,
        Type = CardType.Attack,
        Rarity = CardRarity.Common,
        TooltipText = GenerateTooltipText(BaseDamageAmount),
        IconPath = "res://art/tile_0118.png",
        SoundPath = "res://art/slash.ogg",
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler modifierHandler,
        CancellationToken cancellationToken)
    {
        var modifiedValue = modifierHandler.GetModifiedValue(BaseDamageAmount, ModifierType.DamageDealt);
        var damageEffect = new DamageEffect(modifiedValue)
        {
            Sound = Sound,
        };
        await damageEffect.ExecuteAllAsync(targets, cancellationToken);
    }

    private static string GenerateTooltipText(int damage) =>
        $"[center]Deal [color=\"ff0000\"]{damage}[/color] damage to all enemies.[/center]";

    public override string GetUpdatedTooltipText(ModifierHandler playerModifierHandler,
        ModifierHandler? enemyModifierHandler = null)
    {
        var modifiedDamage = playerModifierHandler.GetModifiedValue(BaseDamageAmount, ModifierType.DamageDealt);
        if (enemyModifierHandler != null)
        {
            modifiedDamage = enemyModifierHandler.GetModifiedValue(modifiedDamage, ModifierType.DamageTaken);
        }

        return GenerateTooltipText(modifiedDamage);
    }
}