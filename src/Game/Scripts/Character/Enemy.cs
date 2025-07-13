using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.EnemyAI;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;

namespace CardGameV1.Character;

[SceneTree]
public partial class Enemy : Area2D, ITarget
{
    private const int ArrowOffset = 5;
    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");

    private EnemyStats _stats = null!;

    private EnemyActionPicker _enemyActionPicker = null!;
    private EnemyAction? _currentAction;

    private readonly CancellationTokenSource ctsOnQueueFree = new();

    public EnemyAction? CurrentAction
    {
        get => _currentAction;
        set
        {
            _currentAction = value;
            UpdateIntent();
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

    public StatusHandler StatusHandler { get; private set; } = null!;

    public ModifierHandler ModifierHandler { get; } = ModifierFactory.CreateEnemyModifierHandler();

    public CancellationToken CancellationTokenOnQueueFree => ctsOnQueueFree.Token;

    #region lifecycle

    public override void _Ready()
    {
        _enemyActionPicker = EnemyActionPickerFactory.CreateCrabBrain(this);
        StatusHandler = new StatusHandler(this, _.StatusContainer);
    }

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
        StatusHandler.OnDispose();
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
            await StatusHandler.ApplyStatusesByType(StatusType.StartOfTurn, CancellationTokenOnQueueFree);

            Stats.Block = 0;
            if (CurrentAction != null)
            {
                await CurrentAction.PerformActionAsync(CancellationTokenOnQueueFree);
            }

            await StatusHandler.ApplyStatusesByType(StatusType.EndOfTurn, CancellationTokenOnQueueFree);
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

    public async Task TakeDamageAsync(int damage, ModifierType whichModifier)
    {
        if (Stats.Health <= 0)
            return;

        _.Sprite2D.Material = WhiteSprite;
        await this.ShakeAsync(16, 0.15f);

        var modifiedDamage = ModifierHandler.GetModifiedValue(damage, whichModifier);
        Stats.TakeDamage(modifiedDamage);
        _.Sprite2D.Material = null;

        if (Stats.Health <= 0)
        {
            QFree();
        }
    }

    private void UpdateEnemy()
    {
        if (IsInsideTree() == false)
            return;

        _.Sprite2D.Texture = Stats.Art;
        var spriteWidth = _.Sprite2D.GetRect().Size.X;
        _.Arrow.Position = Vector2.Right * (spriteWidth / 2 + ArrowOffset);
        UpdateStats();
        UpdateAI();
    }

    private void UpdateAI()
    {
        _enemyActionPicker = EnemyStats.ActionPickerGetter(this);
        var target = (ITarget)GetTree().GetFirstNodeInGroup(GroupNames.Player);
        _enemyActionPicker.SetActionTarget(target);
    }

    private void UpdateStats() => _.StatsUI.UpdateStats(Stats);

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

    public void UpdateIntent()
    {
        if (CurrentAction != null)
        {
            CurrentAction.UpdateIntentText();
            _.IntentUI.UpdateIntent(CurrentAction.Intent);
        }
    }

    private void OnAreaEntered(Area2D area2D)
    {
        _.Arrow.Show();
    }

    private void OnAreaExited(Area2D area2D)
    {
        _.Arrow.Hide();
    }
}