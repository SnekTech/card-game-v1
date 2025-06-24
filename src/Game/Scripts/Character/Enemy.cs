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
    private const int ArrowOffset = 5;

    [Export]
    private EnemyStats originalEnemyStats = null!;

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

    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");

    private Stats _stats = null!;

    // todo: use different action picker for different enemy
    private readonly EnemyActionPicker _enemyActionPicker = EnemyActionPickerFactory.Crab;
    private EnemyAction? _currentAction;

    private CancellationTokenSource cts = new();

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

    public Stats Stats
    {
        get => _stats;
        private set
        {
            _stats = value.CreateInstance();
            _stats.StatsChanged += UpdateStats;
            _stats.StatsChanged += UpdateAction;
            UpdateEnemy();
        }
    }

    public StatusHandler StatusHandler => statusHandler;

    #region lifecycle

    public override void _Ready()
    {
        Stats = originalEnemyStats;

        var target = (ITarget)GetTree().GetFirstNodeInGroup(GroupNames.Player);
        _enemyActionPicker.SetActionTarget(target);
        _enemyActionPicker.SetActionEnemy(this);

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

    #endregion

    protected override void Dispose(bool disposing)
    {
        cts.Cancel();
        base.Dispose(disposing);
    }

    public async Task DoTurnAsync()
    {
        if (cts.IsCancellationRequested)
        {
            cts.Dispose();
            cts = new CancellationTokenSource();
        }

        try
        {
            await statusHandler.ApplyStatusesByType(StatusType.StartOfTurn, cts.Token);

            Stats.Block = 0;
            if (CurrentAction != null)
            {
                await CurrentAction.PerformActionAsync();
            }

            await statusHandler.ApplyStatusesByType(StatusType.EndOfTurn, cts.Token);
        }
        catch (OperationCanceledException)
        {
            GD.Print($"enemy turn canceled, 'cause this enemy died");
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
            QueueFree();
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