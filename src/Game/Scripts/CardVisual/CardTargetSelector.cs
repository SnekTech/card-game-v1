using CardGameV1.EffectSystem;
using CardGameV1.EventBus;

namespace CardGameV1.CardVisual;

[SceneTree]
public partial class CardTargetSelector : Node2D
{
    private const int ArcPoints = 8;

    private static readonly CardEvents CardEvents = EventBusOwner.CardEvents;
    private CardUI? _currentCardUI;

    public override void _EnterTree()
    {
        CardEvents.CardAimStarted += OnCardAimStarted;
        CardEvents.CardAimEnded += OnCardAimEnded;

        Area2D.AreaEntered += OnAreaEntered;
        Area2D.AreaExited += OnAreaExited;
    }

    public override void _ExitTree()
    {
        CardEvents.CardAimStarted -= OnCardAimStarted;
        CardEvents.CardAimEnded -= OnCardAimEnded;

        Area2D.AreaEntered -= OnAreaEntered;
        Area2D.AreaExited -= OnAreaExited;
    }

    public override void _Process(double delta)
    {
        if (_currentCardUI == null)
            return;

        Area2D.Position = GetLocalMousePosition();
        CardArc.Points = GetPoints(_currentCardUI);
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
        Area2D.Monitoring = true;
        Area2D.Monitorable = true;
    }

    private void OnCardAimEnded(CardUI cardUI)
    {
        _currentCardUI = null;
        CardArc.Points = [];
        Area2D.Position = Vector2.Zero;
        Area2D.Monitoring = false; // warning: set Area2D.Monitoring will trigger area exited event
        Area2D.Monitorable = false;
    }

    private void OnAreaEntered(Area2D otherArea2D)
    {
        if (_currentCardUI == null)
            return;

        if (otherArea2D is ITarget target)
        {
            _currentCardUI.Targets.Add(target);
            _currentCardUI.RequestTooltip();
        }
    }

    private void OnAreaExited(Area2D otherArea2D)
    {
        if (_currentCardUI == null)
            return;

        if (otherArea2D is ITarget target)
        {
            _currentCardUI.Targets.Remove(target);
            _currentCardUI.RequestTooltip();
        }
    }
}