using System;
using System.Collections.Generic;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    [Export]
    private Card card = null!;

    [Export]
    private CharacterStats defaultCharacterStats = null!;

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

    private CardStateMachine _stateMachine = null!;
    private readonly HashSet<Node> _targets = [];
    private Tween? _tween;
    private CharacterStats? _characterStats;

    public int OriginalIndex { get; private set; }

    public bool MonitoringDrop
    {
        get => dropPointDetector.Monitoring;
        set => dropPointDetector.Monitoring = value;
    }

    public ISet<Node> Targets => _targets;

    public Card Card
    {
        get => card;
        private set
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
        get => _characterStats ?? defaultCharacterStats;
        set
        {
            if (_characterStats != null)
            {
                _characterStats.StatsChanged -= OnCharacterStatsChanged;
            }
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
        OriginalIndex = GetIndex();
        CharacterStats = defaultCharacterStats;
        var cardEventBus = EventBusOwner.CardEventBus;
        cardEventBus.CardDragStarted += OnCardDragOrAimingStarted;
        cardEventBus.CardDragEnded += OnCardDragOrAimingEnded;
        cardEventBus.CardAimStarted += OnCardDragOrAimingStarted;
        cardEventBus.CardAimEnded += OnCardDragOrAimingEnded;

        InitWithCard(Card);

        _stateMachine = new CardStateMachine(this);
        _stateMachine.Init<CardStates.BaseState>();

        dropPointDetector.MouseEntered += _stateMachine.OnMouseEntered;
        dropPointDetector.MouseExited += _stateMachine.OnMouseExited;

        dropPointDetector.AreaEntered += OnDropPointDetectorAreaEntered;
        dropPointDetector.AreaExited += OnDropPointDetectorAreaExited;
    }

    public override void _Input(InputEvent @event) => _stateMachine.OnInput(@event);
    public override void _GuiInput(InputEvent @event) => _stateMachine.OnGuiInput(@event);

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

    public void Play()
    {
        Card.Play(Targets, CharacterStats);
        ClearSubscriptions();
        QueueFree();
    }

    private void ClearSubscriptions()
    {
        var cardEventBus = EventBusOwner.CardEventBus;
        cardEventBus.CardDragStarted -= OnCardDragOrAimingStarted;
        cardEventBus.CardDragEnded -= OnCardDragOrAimingEnded;
        cardEventBus.CardAimStarted -= OnCardDragOrAimingStarted;
        cardEventBus.CardAimEnded -= OnCardDragOrAimingEnded;

        defaultCharacterStats.StatsChanged -= OnCharacterStatsChanged;

        dropPointDetector.MouseEntered -= _stateMachine.OnMouseEntered;
        dropPointDetector.MouseExited -= _stateMachine.OnMouseExited;

        dropPointDetector.AreaEntered -= OnDropPointDetectorAreaEntered;
        dropPointDetector.AreaExited -= OnDropPointDetectorAreaExited;
    }

    private void InitWithCard(Card cardDefinition)
    {
        cost.Text = cardDefinition.Cost.ToString();
        icon.Texture = cardDefinition.Icon;
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
        GD.Print("set playable");
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}