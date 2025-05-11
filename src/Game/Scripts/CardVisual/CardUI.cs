using System;
using System.Collections.Generic;
using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    [Export]
    private Card card = null!;

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

    public Control Parent { get; set; } = null!;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }

    public override void _Ready()
    {
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

    private void InitWithCard(Card cardDefinition)
    {
        cost.Text = cardDefinition.Cost.ToString();
        icon.Texture = cardDefinition.Icon;
    }

    private void OnDropPointDetectorAreaEntered(Area2D area) => _targets.Add(area);

    private void OnDropPointDetectorAreaExited(Area2D area) => _targets.Remove(area);
}