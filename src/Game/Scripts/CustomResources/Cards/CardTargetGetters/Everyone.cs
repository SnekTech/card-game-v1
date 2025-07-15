using CardGameV1.Constants;
using CardGameV1.EffectSystem;

namespace CardGameV1.CustomResources.Cards.CardTargetGetters;

public class Everyone : ICardTargetGetter
{
    public IEnumerable<ITarget> GetTargets(SceneTree tree, ITarget? aimingTarget)
        => tree.GetNodesInGroup(GroupNames.Player).Concat(tree.GetNodesInGroup(GroupNames.Enemy)).OfType<ITarget>();
}