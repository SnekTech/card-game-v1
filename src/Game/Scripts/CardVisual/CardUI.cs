using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGameV1.CardVisual.CardStates;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    [Node]
    private Panel panel = null!;

    [Node]
    private TextureRect icon = null!;

    [Node]
    private Label cost = null!;

    [Node]
    private Area2D dropPointDetector = null!;

    public static readonly StyleBoxFlat BaseStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_base_stylebox.tres");

    public static readonly StyleBoxFlat HoverStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_hover_stylebox.tres");

    public static readonly StyleBoxFlat DraggingStyleBox =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_dragging_stylebox.tres");

    private static readonly CardEventBus CardEvents = EventBusOwner.CardEvents;

    private CardStateMachine? _stateMachine;
    private readonly HashSet<Node> _targets = [];
    private Tween? _tween;
    private CharacterStats _characterStats = null!;
    private Card card = null!;

    public int OriginalIndex { get; set; }

    public bool MonitoringDrop
    {
        get => dropPointDetector.Monitoring;
        set => dropPointDetector.Monitoring = value;
    }

    public ISet<Node> Targets => _targets;

    public Card Card
    {
        get => card;
        set
        {
            card = value;
            if (IsInsideTree())
            {
                InitWithCard(value);
            }
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
                cost.AddThemeColorOverride(fontColorName, Colors.Red);
                icon.SetModulateAlpha(0.5f);
            }
            else
            {
                cost.RemoveThemeColorOverride(fontColorName);
                icon.SetModulateAlpha(1);
            }
        }
    }

    public bool Disabled { get; set; }

    public override void _Ready()
    {
        _stateMachine = new CardStateMachine(this);
        _stateMachine.Init<BaseState>();
    }

    public override void _EnterTree()
    {
        CardEvents.CardDragStarted += OnCardDragOrAimingStarted;
        CardEvents.CardDragEnded += OnCardDragOrAimingEnded;
        CardEvents.CardAimStarted += OnCardDragOrAimingStarted;
        CardEvents.CardAimEnded += OnCardDragOrAimingEnded;

        dropPointDetector.MouseEntered += OnDropPointDetectorMouseEntered;
        dropPointDetector.MouseExited += OnDropPointDetectorMouseExited;

        dropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
        dropPointDetector.AreaExited += OnDropPointDetectorAreaExited;
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

    public void SetPanelStyleBox(StyleBox styleBox)
    {
        const string panelStylePath = "theme_override_styles/panel";
        panel.Set(panelStylePath, styleBox);
    }

    public async Task PlayAsync()
    {
        await Card.PlayAsync(Targets, CharacterStats);
        QueueFree();
    }

    private void ClearSubscriptions()
    {
        CardEvents.CardDragStarted -= OnCardDragOrAimingStarted;
        CardEvents.CardDragEnded -= OnCardDragOrAimingEnded;
        CardEvents.CardAimStarted -= OnCardDragOrAimingStarted;
        CardEvents.CardAimEnded -= OnCardDragOrAimingEnded;

        dropPointDetector.MouseEntered -= OnDropPointDetectorMouseEntered;
        dropPointDetector.MouseExited -= OnDropPointDetectorMouseExited;

        dropPointDetector.AreaEntered -= OnDropPointDetectorAreaEntered;
        dropPointDetector.AreaExited -= OnDropPointDetectorAreaExited;

        CharacterStats.StatsChanged -= OnCharacterStatsChanged;
    }

    private void InitWithCard(Card cardDefinition)
    {
        cost.Text = cardDefinition.Cost.ToString();
        icon.Texture = cardDefinition.Icon;
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
        Playable = CharacterStats.CanPlayCard(card);
    }

    private void OnCharacterStatsChanged()
    {
        Playable = CharacterStats.CanPlayCard(card);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}