using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class StatsUI : HBoxContainer
{
    [Node]
    private HBoxContainer blockContainer = null!;

    [Node]
    private HBoxContainer healthContainer = null!;

    [Node]
    private Label blockLabel = null!;

    [Node]
    private Label healthLabel = null!;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }

    public void UpdateStats(Stats stats)
    {
        var (block, health) = (stats.Block, stats.Health);

        blockLabel.Text = block.ToString();
        healthLabel.Text = health.ToString();

        blockContainer.Visible = block > 0;
        healthContainer.Visible = health > 0;
    }
}