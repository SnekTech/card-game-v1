using GodotUtilities;

namespace CardGameV1.StatusSystem.UI;

[Scene]
public partial class StatusTooltip : HBoxContainer
{
    [Node]
    private TextureRect icon = null!;
    [Node]
    private Label label = null!;

    public void Update(Status status)
    {
        icon.Texture = status.Icon;
        label.Text = status.Tooltip;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}