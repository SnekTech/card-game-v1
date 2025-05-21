using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class RedFlash : CanvasLayer
{
    [Node]
    private ColorRect colorRect = null!;

    [Node]
    private Timer timer = null!;

    public override void _Ready()
    {
        EventBus.EventBusOwner.PlayerEventBus.PlayerHit += OnPlayerHit;
        timer.Timeout += OnTimerTimeout;
    }

    private void OnPlayerHit()
    {
        colorRect.Color = colorRect.Color with { A = 0.2f };
        timer.Start();
    }

    private void OnTimerTimeout()
    {
        colorRect.Color = colorRect.Color with { A = 0 };
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}