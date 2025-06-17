using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class HealthUI : HBoxContainer
{
    [Export]
    private bool showMaxHp;

    [Node]
    private Label healthLabel = null!;
    [Node]
    private Label maxHealthLabel = null!;

    public void UpdateStats(Stats stats)
    {
        healthLabel.Text = stats.Health.ToString();
        maxHealthLabel.Text = $"/{stats.MaxHealth}";
        maxHealthLabel.Visible = showMaxHp;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}