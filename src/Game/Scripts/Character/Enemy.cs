using System;
using System.Threading;
using System.Threading.Tasks;
using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.EnemyAI;
using CardGameV1.StatusSystem;
using CardGameV1.UI.BattleUIComponents;
using Godot;
using GodotUtilities;

namespace CardGameV1.Character;

[Scene]
public partial class Enemy : Area2D, ITarget
{
    [Node]
    private Sprite2D sprite2D = null!;
    [Node]
    private Sprite2D arrow = null!;
    [Node]
    private StatsUI statsUI = null!;
    [Node]
    private IntentUI intentUI = null!;
    [Node]
    private StatusHandler statusHandler = null!;

    private const int ArrowOffset = 5;
    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");

    private EnemyStats _stats = null!;

    private EnemyActionPicker _enemyActionPicker = EnemyActionPickerFactory.CreateCrabBrain();
    private EnemyAction? _currentAction;

    private CancellationTokenSource ctsOnQueueFree = new();

    public EnemyAction? CurrentAction
    {
        get => _currentAction;
        set
        {
            _currentAction = value;
            if (_currentAction != null)
            {
                intentUI.UpdateIntent(_currentAction.Intent);
            }
        }
    }

    public Stats Stats => _stats;

    public EnemyStats EnemyStats
    {
        get => _stats;
        set
        {
            _stats = value;
            _stats.StatsChanged += UpdateStats;
            _stats.StatsChanged += UpdateAction;
            UpdateEnemy();
        }
    }

    public StatusHandler StatusHandler => statusHandler;

    public CancellationToken CancellationTokenOnQueueFree => ctsOnQueueFree.Token;

    #region lifecycle

    public override void _EnterTree()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
        AreaExited -= OnAreaExited;
        _stats.StatsChanged -= UpdateStats;
        _stats.StatsChanged -= UpdateAction;
    }

    public void QFree()
    {
        ctsOnQueueFree.Cancel();
        QueueFree();
    }

    #endregion

    public async Task DoTurnAsync()
    {
        try
        {
            await statusHandler.ApplyStatusesByType(StatusType.StartOfTurn, CancellationTokenOnQueueFree);

            Stats.Block = 0;
            if (CurrentAction != null)
            {
                await CurrentAction.PerformActionAsync(CancellationTokenOnQueueFree);
            }

            await statusHandler.ApplyStatusesByType(StatusType.EndOfTurn, CancellationTokenOnQueueFree);
        }
        catch (OperationCanceledException e)
        {
            if (e.CancellationToken == ctsOnQueueFree.Token)
            {
                GD.Print($"enemy turn canceled, 'cause this enemy died");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task TakeDamageAsync(int damage)
    {
        if (Stats.Health <= 0)
            return;

        sprite2D.Material = WhiteSprite;
        await this.ShakeAsync(16, 0.15f);
        Stats.TakeDamage(damage);
        sprite2D.Material = null;

        if (Stats.Health <= 0)
        {
            QFree();
        }
    }

    private void UpdateEnemy()
    {
        if (IsInsideTree() == false)
            return;

        sprite2D.Texture = Stats.Art;
        var spriteWidth = sprite2D.GetRect().Size.X;
        arrow.Position = Vector2.Right * (spriteWidth / 2 + ArrowOffset);
        UpdateStats();
        UpdateAI();
    }

    private void UpdateAI()
    {
        _enemyActionPicker = EnemyStats.ActionPickerGetter();
        var target = (ITarget)GetTree().GetFirstNodeInGroup(GroupNames.Player);
        _enemyActionPicker.SetActionTarget(target);
        _enemyActionPicker.SetActionEnemy(this);
    }

    private void UpdateStats() => statsUI.UpdateStats(Stats);

    public void UpdateAction()
    {
        if (CurrentAction == null)
        {
            CurrentAction = _enemyActionPicker.GetAction();
            return;
        }

        var newConditionalAction = _enemyActionPicker.GetFirstConditionalAction();
        if (newConditionalAction != null && CurrentAction != newConditionalAction)
        {
            CurrentAction = newConditionalAction;
        }
    }

    private void OnAreaEntered(Area2D area2D)
    {
        arrow.Show();
    }

    private void OnAreaExited(Area2D area2D)
    {
        arrow.Hide();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}