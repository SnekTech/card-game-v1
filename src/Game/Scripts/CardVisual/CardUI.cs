using CardGameV1.CardVisual.CardStates;
using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;
using CardGameV1.CustomResources.Cards.Warrior;
using CardGameV1.EventBus;
using CardGameV1.ModifierSystem;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[SceneTree]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    public static readonly StyleBoxFlat BaseStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_base_stylebox.tres");

    public static readonly StyleBoxFlat HoverStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_hover_stylebox.tres");

    public static readonly StyleBoxFlat DraggingStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_dragging_stylebox.tres");

    private static readonly CardEvents CardEvents = EventBusOwner.CardEvents;

    private CardStateMachine? _stateMachine;
    private readonly HashSet<Node> _targets = [];
    private Tween? _tween;
    private CharacterStats _characterStats = null!;

    private readonly Card defaultCard = CardPool.Get<WarriorAxeAttack>();
    private Card? _card;

    public int OriginalIndex { get; set; }

    public bool MonitoringDrop
    {
        get => DropPointDetector.Monitoring;
        set => DropPointDetector.Monitoring = value;
    }

    public ISet<Node> Targets => _targets;

    public Card Card
    {
        get => _card ?? defaultCard;
        set
        {
            _card = value;
            _.CardVisuals.Card = _card;
        }
    }

    public CharacterStats CharacterStats
    {
        get => _characterStats;
        set
        {
            _characterStats = value;
            _characterStats.StatsChanged += OnCharacterStatsChanged;
        }
    }

    public Control Parent { get; set; } = null!;

    public ModifierHandler PlayerModifierHandler { private get; set; } = null!;

    private bool _playable = true;

    public bool Playable
    {
        get => _playable;
        private set
        {
            _playable = value;
            var fontColorName = new StringName("font_color");
            if (_playable == false)
            {
                CardVisuals.Cost.AddThemeColorOverride(fontColorName, Colors.Red);
                CardVisuals.Icon.SetModulateAlpha(0.5f);
            }
            else
            {
                CardVisuals.Cost.RemoveThemeColorOverride(fontColorName);
                CardVisuals.Icon.SetModulateAlpha(1);
            }
        }
    }

    public bool Disabled { get; set; }

    public override void _Ready()
    {
        Card = defaultCard;

        _stateMachine = new CardStateMachine(this);
        _stateMachine.Init<BaseState>();
    }

    public override void _EnterTree()
    {
        CardEvents.CardDragStarted += OnCardDragOrAimingStarted;
        CardEvents.CardDragEnded += OnCardDragOrAimingEnded;
        CardEvents.CardAimStarted += OnCardDragOrAimingStarted;
        CardEvents.CardAimEnded += OnCardDragOrAimingEnded;

        DropPointDetector.MouseEntered += OnDropPointDetectorMouseEntered;
        DropPointDetector.MouseExited += OnDropPointDetectorMouseExited;

        DropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
        DropPointDetector.AreaExited += OnDropPointDetectorAreaExited;
    }

    public override void _ExitTree()
    {
        ClearSubscriptions();
    }

    public override void _Input(InputEvent @event) => _stateMachine?.OnInput(@event);
    public override void _GuiInput(InputEvent @event) => _stateMachine?.OnGuiInput(@event);

    public void EmitReparentRequested() => ReparentRequested?.Invoke(this);

    public void AnimateToPosition(Vector2 destination, float duration)
    {
        _tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
        _tween.TweenProperty(this, new NodePath(Control.PropertyName.GlobalPosition), destination, duration);
    }

    public void StopAnimation()
    {
        if (_tween != null && _tween.IsRunning())
            _tween.KillIfValid();
    }

    public void SetPanelStyleBox(StyleBox styleBox) => CardVisuals.Panel.SetStyleBox(styleBox);

    public async Task PlayAsync(CancellationToken cancellationToken)
    {
        await Card.PlayAsync(Targets, CharacterStats, PlayerModifierHandler, cancellationToken);
        QueueFree();
    }

    public void RequestTooltip()
    {
        var enemyModifierHandler = GetActiveEnemyModifierHandler();
        var updatedTooltip = Card.GetUpdatedTooltipText(PlayerModifierHandler, enemyModifierHandler);
        CardEvents.EmitCardTooltipRequested(Card.Icon, updatedTooltip);
        return;

        ModifierHandler? GetActiveEnemyModifierHandler()
        {
            if (Targets.Count != 1)
                return null;
            if (Targets.First() is not Enemy enemy)
                return null;

            return enemy.ModifierHandler;
        }
    }

    private void ClearSubscriptions()
    {
        CardEvents.CardDragStarted -= OnCardDragOrAimingStarted;
        CardEvents.CardDragEnded -= OnCardDragOrAimingEnded;
        CardEvents.CardAimStarted -= OnCardDragOrAimingStarted;
        CardEvents.CardAimEnded -= OnCardDragOrAimingEnded;

        DropPointDetector.MouseEntered -= OnDropPointDetectorMouseEntered;
        DropPointDetector.MouseExited -= OnDropPointDetectorMouseExited;

        DropPointDetector.AreaEntered -= OnDropPointDetectorAreaEntered;
        DropPointDetector.AreaExited -= OnDropPointDetectorAreaExited;

        CharacterStats.StatsChanged -= OnCharacterStatsChanged;
    }

    private void OnDropPointDetectorMouseEntered()
    {
        _stateMachine?.OnMouseEntered();
    }

    private void OnDropPointDetectorMouseExited()
    {
        _stateMachine?.OnMouseExited();
    }

    private void OnDropPointDetectorAreaEntered(Area2D area) => _targets.Add(area);

    private void OnDropPointDetectorAreaExited(Area2D area) => _targets.Remove(area);

    private void OnCardDragOrAimingStarted(CardUI cardUI)
    {
        if (cardUI != this)
            return;

        Disabled = true;
    }

    private void OnCardDragOrAimingEnded(CardUI cardUI)
    {
        Disabled = false;
        Playable = CharacterStats.CanPlayCard(Card);
    }

    private void OnCharacterStatsChanged()
    {
        Playable = CharacterStats.CanPlayCard(Card);
    }
}