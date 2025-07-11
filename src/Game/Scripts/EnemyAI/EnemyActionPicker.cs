using CardGameV1.EffectSystem;

namespace CardGameV1.EnemyAI;

public class EnemyActionPicker
{
    public EnemyActionPicker(
        List<EnemyConditionalAction> conditionalActions,
        List<EnemyChanceBasedAction> chanceBasedActions)
    {
        _conditionalActions = new List<EnemyConditionalAction>(conditionalActions);
        _chanceBasedActions = new List<EnemyChanceBasedAction>(chanceBasedActions);
        SetupChances();
    }

    private readonly List<EnemyConditionalAction> _conditionalActions;
    private readonly List<EnemyChanceBasedAction> _chanceBasedActions;

    private float _totalWeight;

    public EnemyAction GetAction()
    {
        var firstConditionalAction = GetFirstConditionalAction();
        if (firstConditionalAction != null)
        {
            return firstConditionalAction;
        }

        return GetChanceBasedAction();
    }

    public EnemyConditionalAction? GetFirstConditionalAction()
    {
        return _conditionalActions.FirstOrDefault(conditionalAction => conditionalAction.IsPerformable());
    }

    public void SetActionTarget(ITarget target)
    {
        foreach (var action in _conditionalActions) action.UpdateTarget(target);
        foreach (var action in _chanceBasedActions) action.UpdateTarget(target);
    }

    private EnemyChanceBasedAction GetChanceBasedAction()
    {
        var roll = GD.RandRange(0f, _totalWeight);
        foreach (var chanceBasedAction in _chanceBasedActions)
        {
            if (chanceBasedAction.AccumulatedWeight > roll)
            {
                return chanceBasedAction;
            }
        }

        return _chanceBasedActions[0]; // use the first as default
    }

    private void SetupChances()
    {
        foreach (var chanceBasedAction in _chanceBasedActions)
        {
            _totalWeight += chanceBasedAction.ChanceWeight;
            chanceBasedAction.AccumulatedWeight = _totalWeight;
        }
    }
}