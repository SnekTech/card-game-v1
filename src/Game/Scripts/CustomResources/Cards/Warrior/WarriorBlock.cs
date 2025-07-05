using System.Threading;
using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;

namespace CardGameV1.CustomResources.Cards.Warrior;

public class WarriorBlock : Card
{
    private const int BlockAmount = 5;

    protected override CardAttributes Attributes { get; } = new()
    {
        Id = nameof(WarriorBlock),
        Cost = 1,
        Type = CardType.Skill,
        Rarity = CardRarity.Common,
        Target = CardTarget.Self,
        TooltipText = GenerateTooltipText(BlockAmount),
        IconPath = "res://art/tile_0102.png",
        SoundPath = "res://art/block.ogg",
    };

    protected override async Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler _,
        CancellationToken cancellationToken)
    {
        var blockEffect = new BlockEffect(BlockAmount)
        {
            Sound = Sound,
        };
        await blockEffect.ExecuteAllAsync(targets, cancellationToken);
    }

    private static string GenerateTooltipText(int block) =>
        $"[center]Gain [color=\"0044ff\"]{block}[/color] block.[/center]";

    public override string GetUpdatedTooltipText(ModifierHandler playerModifierHandler,
        ModifierHandler enemyModifierHandler) =>
        GenerateTooltipText(BlockAmount);
}