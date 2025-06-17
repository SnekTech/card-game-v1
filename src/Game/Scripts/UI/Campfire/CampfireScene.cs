using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.Campfire;

[Scene]
public partial class CampfireScene : Control
{
    [Node]
    private Button restButton = null!;

    private readonly CharacterStats _characterStats = GD.Load<CharacterStats>("res://characters/warrior/warrior.tres");

    public override void _EnterTree()
    {
        restButton.Pressed += OnRestButtonPressed;
    }

    public override void _ExitTree()
    {
        restButton.Pressed -= OnRestButtonPressed;
    }

    private void OnRestButtonPressed()
    {
        _characterStats.Heal(Mathf.CeilToInt(_characterStats.MaxHealth));
        FadeOut().Fire();
    }

    private async Task FadeOut()
    {
        GD.Print("resting...");
        await SnekUtility.DelayGd(2);
        GD.Print("rest complete");
        EventBusOwner.Events.EmitCampfireExited();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}