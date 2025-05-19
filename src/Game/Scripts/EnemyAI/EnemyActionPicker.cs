using System.Collections.Generic;
using System.Linq;
using CardGameV1.Character;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public class EnemyActionPicker
{
    public EnemyActionPicker()
    {
        SetupChances();
    }

    public Enemy? Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            SetActionEnemy(_enemy);
        }
    }

    public ITarget? Target
    {
        get => _target;
        set
        {
            _target = value;
            SetActionTarget(_target);
        }
    }

    private List<EnemyConditionalAction> ConditionalActions { get; } = [
        new CrabMegaBlockAction()
    ];

    private List<EnemyChanceBasedAction> ChanceBasedActions { get; } =
    [
        new CrabAttackAction { ChanceWeight = 1, Damage = 7 },
        new CrabBlockAction { ChanceWeight = 1, Block = 6 }
    ];

    private float _totalWeight;
    private Enemy? _enemy;
    private ITarget? _target;

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
        return ConditionalActions.FirstOrDefault(conditionalAction => conditionalAction.IsPerformable());
    }

    private EnemyChanceBasedAction GetChanceBasedAction()
    {
        var roll = GD.RandRange(0f, _totalWeight);
        foreach (var chanceBasedAction in ChanceBasedActions)
        {
            if (chanceBasedAction.AccumulatedWeight > roll)
            {
                return chanceBasedAction;
            }
        }

        return ChanceBasedActions[0]; // use the first as default
    }

    private void SetupChances()
    {
        foreach (var chanceBasedAction in ChanceBasedActions)
        {
            _totalWeight += chanceBasedAction.ChanceWeight;
            chanceBasedAction.AccumulatedWeight = _totalWeight;
        }
    }

    private void SetActionEnemy(Enemy? enemy)
    {
        ConditionalActions.ForEach(action => action.Enemy = enemy);
        ChanceBasedActions.ForEach(action => action.Enemy = enemy);
    }

    private void SetActionTarget(ITarget? target)
    {
        ConditionalActions.ForEach(action => action.Target = target);
        ChanceBasedActions.ForEach(action => action.Target = target);
    }
}