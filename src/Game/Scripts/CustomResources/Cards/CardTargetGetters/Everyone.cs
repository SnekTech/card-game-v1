using CardGameV1.Constants;
using CardGameV1.EffectSystem;

namespace CardGameV1.CustomResources.Cards.CardTargetGetters;

public class Everyone : ICardTargetGetter
{
    public IEnumerable<ITarget> GetTargets(SceneTree tree, ITarget? aimingTarget)
    {
        var players = tree.GetNodesInGroup(GroupNames.Player);
        var enemies = tree.GetNodesInGroup(GroupNames.Enemy);
        IEnumerable<Node> everyone = [..players, ..enemies];
        return everyone.OfType<ITarget>();
    }
}