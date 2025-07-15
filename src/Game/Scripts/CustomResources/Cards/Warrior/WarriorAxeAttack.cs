using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;

namespace CardGameV1.CustomResources.Cards.Warrior;

public class WarriorAxeAttack : Card
{
    private const int BaseDamageAmount = 6;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorAxeAttack),
        Cost = 1,
        Type = CardType.Attack,
        Rarity = CardRarity.Common,
        TooltipText = GenerateTooltipText(BaseDamageAmount),
        IconPath = "res://art/tile_0119.png",
        SoundPath = "res://art/axe.ogg",
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler modifierHandler,
        CancellationToken cancellationToken)
    {
        var modifiedDamage = modifierHandler.GetModifiedValue(BaseDamageAmount, ModifierType.DamageDealt);
        var damageEffect = new DamageEffect(modifiedDamage)
        {
            Sound = Sound,
        };
        await damageEffect.ExecuteAllAsync(targets, cancellationToken);
    }

    private static string GenerateTooltipText(int damage) =>
        $"[center]Deal [color=\"ff0000\"]{damage}[/color] damage.[/center]";

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