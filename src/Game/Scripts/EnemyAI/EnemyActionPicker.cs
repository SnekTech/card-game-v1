using System.Collections.Generic;
using System.Linq;
using CardGameV1.Character;
using CardGameV1.EffectSystem;
using Godot;
using GodotUtilities;

namespace CardGameV1.EnemyAI;

public partial class EnemyActionPicker : Node
{
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

    private List<EnemyConditionalAction> ConditionalActions { get; } = [];

    private List<EnemyChanceBasedAction> ChanceBasedActions { get; } = [];

    private float _totalWeight;
    private Enemy? _enemy;
    private ITarget? _target;

    public override void _Ready()
    {
        var conditionalActionChildren = this.GetChildrenOfType<EnemyConditionalAction>();
        var chanceBasedActionChildren = this.GetChildrenOfType<EnemyChanceBasedAction>();
        ConditionalActions.AddRange(conditionalActionChildren);
        ChanceBasedActions.AddRange(chanceBasedActionChildren);
        GD.Print($"{ConditionalActions}, {ChanceBasedActions}");

        SetupChances();
    }

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