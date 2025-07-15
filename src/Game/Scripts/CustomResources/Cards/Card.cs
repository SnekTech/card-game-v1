using CardGameV1.Character;
using CardGameV1.Constants;
using CardGameV1.CustomResources.Cards.CardTargetGetters;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using CardGameV1.ModifierSystem;

namespace CardGameV1.CustomResources.Cards;

public abstract class Card
{
    public string Id => Attributes.Id;
    public int Cost => Attributes.Cost;
    public CardType Type => Attributes.Type;
    public CardRarity Rarity => Attributes.Rarity;
    public bool ShouldExhaust => Attributes.ShouldExhaust;

    public string TooltipText => Attributes.TooltipText;
    public Texture2D Icon => SnekUtility.LoadTexture(Attributes.IconPath);
    public AudioStream Sound => SnekUtility.LoadSound(Attributes.SoundPath);

    public required ICardTargetGetter TargetGetter { get; init; }
    public bool IsSingleTargeted => TargetGetter is SingleEnemy;

    public async Task PlayAsync(SceneTree tree, ITarget? aimingTarget, CharacterStats characterStats,
        ModifierHandler playerModifierHandler,
        CancellationToken cancellationToken)
    {
        EventBusOwner.CardEvents.EmitCardPlayed(this);
        characterStats.Mana -= Cost;
        await ApplyEffectsAsync(TargetGetter.GetTargets(tree, aimingTarget), playerModifierHandler, cancellationToken);
    }

    protected abstract CardAttributes Attributes { get; }

    protected abstract Task ApplyEffectsAsync(IEnumerable<ITarget> targets, ModifierHandler modifiers,
        CancellationToken cancellationToken);

    public abstract string GetUpdatedTooltipText(ModifierHandler playerModifierHandler,
        ModifierHandler? enemyModifierHandler = null);
}