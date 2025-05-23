using System.Collections.Generic;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardTargetSelector : Node2D
{
    private const int ArcPoints = 8;

    [Node]
    private Area2D area2D = null!;

    [Node]
    private Line2D cardArc = null!;

    private readonly CardEventBus _eventBus = EventBusOwner.CardEvents;
    private CardUI? _currentCardUI;

    public override void _EnterTree()
    {
        _eventBus.CardAimStarted += OnCardAimStarted;
        _eventBus.CardAimEnded += OnCardAimEnded;

        area2D.AreaEntered += OnAreaEntered;
        area2D.AreaExited += OnAreaExited;
    }

    public override void _ExitTree()
    {
        _eventBus.CardAimStarted -= OnCardAimStarted;
        _eventBus.CardAimEnded -= OnCardAimEnded;

        area2D.AreaEntered -= OnAreaEntered;
        area2D.AreaExited -= OnAreaExited;
    }

    public override void _Process(double delta)
    {
        if (_currentCardUI == null)
            return;

        area2D.Position = GetLocalMousePosition();
        cardArc.Points = GetPoints(_currentCardUI);
    }

    private Vector2[] GetPoints(CardUI cardUI)
    {
        var points = new List<Vector2>();
        var start = cardUI.GlobalPosition;
        start.X += cardUI.Size.X / 2;
        var target = GetLocalMousePosition();
        var distance = target - start;

        for (var i = 0; i < ArcPoints; i++)
        {
            var t = 1.0f / ArcPoints * i;
            var x = start.X + distance.X / ArcPoints * i;
            var y = start.Y + EaseOutCubic(t) * distance.Y;
            points.Add(new Vector2(x, y));
        }

        return points.ToArray();
    }

    private static float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);

    private void OnCardAimStarted(CardUI cardUI)
    {
        _currentCardUI = cardUI;
        area2D.Monitoring = true;
        area2D.Monitorable = true;
    }

    private void OnCardAimEnded(CardUI cardUI)
    {
        _currentCardUI = null;
        cardArc.Points = [];
        area2D.Position = Vector2.Zero;
        area2D.Monitoring = false; // warning: set Area2D.Monitoring will trigger area exited event
        area2D.Monitorable = false;
    }

    private void OnAreaEntered(Area2D otherArea2D)
    {
        if (_currentCardUI == null)
            return;

        _currentCardUI.Targets.Add(otherArea2D);
    }

    private void OnAreaExited(Area2D otherArea2D)
    {
        if (_currentCardUI == null)
            return;

        _currentCardUI.Targets.Remove(otherArea2D);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}