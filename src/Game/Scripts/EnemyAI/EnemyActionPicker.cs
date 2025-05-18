using System.Collections.Generic;
using System.Linq;
using CardGameV1.Character;
using CardGameV1.Constants;
using Godot;
using GodotUtilities;

namespace CardGameV1.EnemyAI;

[GlobalClass]
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

    public Node2D Target
    {
        get => _target;
        set
        {
            _target = value;
            SetActionTarget(_target);
        }
    }

    public List<EnemyConditionalAction> ConditionalActions { get; set; } = [];
    public List<EnemyChanceBasedAction> ChanceBasedActions { get; set; } = [];

    private float _totalWeight;
    private Enemy? _enemy;
    private Node2D _target = null!;

    public override void _Ready()
    {
        Target = GetTree().GetFirstNodeInGroup<Node2D>(GroupNames.Player);
        var crabAttackAction = new CrabAttackAction();
        var crabBlockAction = new CrabBlockAction();

        ChanceBasedActions.Add(crabAttackAction);
        ChanceBasedActions.Add(crabBlockAction);

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

    private EnemyConditionalAction? GetFirstConditionalAction()
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

    private void SetActionTarget(Node2D target)
    {
        ConditionalActions.ForEach(action => action.Target = target);
        ChanceBasedActions.ForEach(action => action.Target = target);
    }
}