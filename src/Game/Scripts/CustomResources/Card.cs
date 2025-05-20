using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGameV1.Constants;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using Godot;
using Godot.Collections;

namespace CardGameV1.CustomResources;

public abstract partial class Card : Resource
{
    [ExportGroup("Card Attribute")]
    [Export]
    public string Id { get; private set; } = "default";

    [Export]
    public int Cost { get; private set; } = 1;

    [Export]
    public CardType Type { get; private set; }

    [Export]
    public TargetType Target { get; private set; }

    [ExportGroup("Card Visual")]
    [Export]
    public Texture2D Icon { get; private set; } = null!;

    [Export(PropertyHint.MultilineText)]
    public string TooltipText { get; private set; } = "default tooltip";

    public bool IsSingleTargeted => Target == TargetType.SingleEnemy;

    private IEnumerable<ITarget> GetTargets(SceneTree tree)
    {
        var targets = Target switch
        {
            TargetType.Self => tree.GetNodesInGroup(GroupNames.Player),
            TargetType.AllEnemies => tree.GetNodesInGroup(GroupNames.Enemy),
            TargetType.Everyone => tree.GetNodesInGroup(GroupNames.Player)
                .Concat(tree.GetNodesInGroup(GroupNames.Enemy)),
            _ => new Array<Node>()
        };
        return targets.Where(target => target is ITarget).Cast<ITarget>();
    }

    public async Task PlayAsync(IEnumerable<Node> targetNodes, CharacterStats characterStats)
    {
        EventBusOwner.CardEventBus.EmitCardPlayed(this);
        characterStats.Mana -= Cost;


        if (IsSingleTargeted)
        {
            await ApplyEffectsAsync(targetNodes.Where(node => node is ITarget).Cast<ITarget>());
        }
        else
        {
            await ApplyEffectsAsync(GetTargets(targetNodes.First().GetTree()));
        }
    }

    protected abstract Task ApplyEffectsAsync(IEnumerable<ITarget> targets);

    #region card enums

    public enum CardType
    {
        Attack,
        Skill,
        Power
    }

    public enum TargetType
    {
        Self,
        SingleEnemy,
        AllEnemies,
        Everyone
    }

    #endregion
}