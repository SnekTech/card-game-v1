using CardGameV1.EffectSystem;

namespace CardGameV1.CustomResources.Cards.CardTargetGetters;

public class SingleEnemy : ICardTargetGetter
{
    public IEnumerable<ITarget> GetTargets(SceneTree tree, ITarget? aimingTarget) 
        => aimingTarget is null ? [] : [aimingTarget];
}