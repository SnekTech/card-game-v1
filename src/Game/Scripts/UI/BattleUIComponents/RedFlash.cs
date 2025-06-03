using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class RedFlash : CanvasLayer
{
    [Node]
    private ColorRect colorRect = null!;

    [Node]
    private Timer timer = null!;

    public override void _EnterTree()
    {
        EventBus.EventBusOwner.PlayerEvents.PlayerHit += OnPlayerHit;
        timer.Timeout += OnTimerTimeout;
    }

    public override void _ExitTree()
    {
        EventBus.EventBusOwner.PlayerEvents.PlayerHit -= OnPlayerHit;
        timer.Timeout -= OnTimerTimeout;
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