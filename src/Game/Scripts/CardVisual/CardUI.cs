﻿using System;
using System.Collections.Generic;
using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    [Export] private Card card = null!;

    [Node] private ColorRect colorRect = null!;
    [Node] private Label stateLabel = null!;
    [Node] private Area2D dropPointDetector = null!;

    private CardStateMachine _stateMachine = null!;
    private readonly HashSet<Node> _targets = [];
    private Tween? _tween;

    public bool MonitoringDrop
    {
        get => dropPointDetector.Monitoring;
        set => dropPointDetector.Monitoring = value;
    }

    public ISet<Node> Targets => _targets;
    public Card Card => card;

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

    public void SetDebugInfo(Color color, string stateName)
    {
        colorRect.Color = color;
        stateLabel.Text = stateName;
    }

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

    private void OnDropPointDetectorAreaEntered(Area2D area) => _targets.Add(area);

    private void OnDropPointDetectorAreaExited(Area2D area) => _targets.Remove(area);
}