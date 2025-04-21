using System;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardUI;

[Scene]
public partial class CardUI : Control
{
    public event Action<CardUI>? ReparentRequested;

    [Node] private ColorRect colorRect = null!;
    [Node] private Label stateLabel = null!;
    [Node] private Area2D dropPointDetector = null!;

    private CardDragStateMachine _dragStateMachine = null!;

    public bool MonitoringDrop
    {
        get => dropPointDetector.Monitoring;
        set => dropPointDetector.Monitoring = value;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }

    public override void _Ready()
    {
        _dragStateMachine = new CardDragStateMachine(this);
        _dragStateMachine.Init<CardStates.BaseState>();
        
        dropPointDetector.MouseEntered += _dragStateMachine.OnMouseEntered;
        dropPointDetector.MouseExited += _dragStateMachine.OnMouseExited;
    }

    public override void _Input(InputEvent @event) => _dragStateMachine.OnInput(@event);
    public override void _GuiInput(InputEvent @event) => _dragStateMachine.OnGuiInput(@event);

    public void EmitReparentRequested() => ReparentRequested?.Invoke(this);

    public void SetDebugInfo(Color color, string stateName)
    {
        colorRect.Color = color;
        stateLabel.Text = stateName;
    }
}