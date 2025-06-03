using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[Scene]
public partial class BattleReward : Control
{
    [Node]
    private Button backButton = null!;

    public override void _EnterTree()
    {
        backButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        backButton.Pressed -= OnBackButtonPressed;
    }

    private void OnBackButtonPressed() => EventBusOwner.Events.EmitBattleRewardExited();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}