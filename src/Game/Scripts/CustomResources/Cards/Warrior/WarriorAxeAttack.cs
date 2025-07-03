using System.Threading;
using System.Threading.Tasks;
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
        Target = CardTarget.SingleEnemy,
        TooltipText = $"[center]Deal [color=\"ff0000\"]{BaseDamageAmount}[/color] damage.[/center]",
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
}