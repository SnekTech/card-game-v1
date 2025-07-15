using CardGameV1.EffectSystem;

namespace CardGameV1.CustomResources.Cards;

public interface ICardTargetGetter
{
    IEnumerable<ITarget> GetTargets(SceneTree tree, ITarget? aimingTarget);
}