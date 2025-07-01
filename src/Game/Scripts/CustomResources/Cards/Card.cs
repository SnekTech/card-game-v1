using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardGameV1.Constants;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using Godot.Collections;

namespace CardGameV1.CustomResources.Cards;

public abstract class Card
{
    public string Id => Attributes.Id;
    public int Cost => Attributes.Cost;
    public CardType Type => Attributes.Type;
    public CardRarity Rarity => Attributes.Rarity;
    public CardTarget Target => Attributes.Target;
    public bool ShouldExhaust => Attributes.ShouldExhaust;

    public string TooltipText => Attributes.TooltipText;
    public Texture2D Icon => SnekUtility.LoadTexture(Attributes.IconPath);
    public AudioStream Sound => SnekUtility.LoadSound(Attributes.SoundPath);

    public bool IsSingleTargeted => Target == CardTarget.SingleEnemy;

    private static readonly System.Collections.Generic.Dictionary<CardRarity, Color> RarityColors = new()
    {
        [CardRarity.Common] = Colors.Gray,
        [CardRarity.Uncommon] = Colors.CornflowerBlue,
        [CardRarity.Rare] = Colors.Gold
    };

    public Color RarityColor => RarityColors[Rarity];

    private IEnumerable<ITarget> GetTargets(SceneTree tree)
    {
        var targets = Target switch
        {
            CardTarget.Self => tree.GetNodesInGroup(GroupNames.Player),
            CardTarget.AllEnemies => tree.GetNodesInGroup(GroupNames.Enemy),
            CardTarget.Everyone => tree.GetNodesInGroup(GroupNames.Player)
                .Concat(tree.GetNodesInGroup(GroupNames.Enemy)),
            _ => new Array<Node>()
        };
        return targets.OfType<ITarget>();
    }

    public async Task PlayAsync(IEnumerable<Node> targetNodes, CharacterStats characterStats,
        CancellationToken cancellationToken)
    {
        EventBusOwner.CardEvents.EmitCardPlayed(this);
        characterStats.Mana -= Cost;


        if (IsSingleTargeted)
        {
            await ApplyEffectsAsync(targetNodes.Where(node => node is ITarget).Cast<ITarget>(), cancellationToken);
        }
        else
        {
            await ApplyEffectsAsync(GetTargets(targetNodes.First().GetTree()), cancellationToken);
        }
    }

    protected abstract CardAttributes Attributes { get; }

    protected abstract Task ApplyEffectsAsync(IEnumerable<ITarget> targets, CancellationToken cancellationToken);
}