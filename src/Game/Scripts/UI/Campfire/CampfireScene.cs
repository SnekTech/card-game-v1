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
    [Node]
    private AnimationPlayer animationPlayer = null!;

    public CharacterStats CharacterStats { get; set; } = null!;

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
        restButton.Disabled = true;
        CharacterStats.Heal(Mathf.CeilToInt(CharacterStats.MaxHealth * 0.3));
        animationPlayer.Play("fade_out");
    }

    // called by the animation player
    private void OnFadeOutAnimationFinished()
    {
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