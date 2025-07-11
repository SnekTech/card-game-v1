using CardGameV1.EffectSystem;
using CardGameV1.WeightedRandom;

namespace CardGameV1.EnemyAI;

public class EnemyActionPicker(
    List<EnemyConditionalAction> conditionalActions,
    List<EnemyChanceBasedAction> chanceBasedActions)
{
    private readonly List<EnemyConditionalAction> _conditionalActions = [..conditionalActions];
    private readonly List<EnemyChanceBasedAction> _chanceBasedActions = [..chanceBasedActions];

    public EnemyAction GetAction()
    {
        var firstConditionalAction = GetFirstConditionalAction();
        if (firstConditionalAction != null)
        {
            return firstConditionalAction;
        }

        return WeightedRandomCalculator.GetWeighted(_chanceBasedActions)!;
    }

    public EnemyConditionalAction? GetFirstConditionalAction()
    {
        return _conditionalActions.FirstOrDefault(conditionalAction =>
            conditionalAction.PerformDictator.IsPerformable());
    }

    public void SetActionTarget(ITarget target)
    {
        foreach (var action in _conditionalActions) action.UpdateTarget(target);
        foreach (var action in _chanceBasedActions) action.UpdateTarget(target);
    }
}