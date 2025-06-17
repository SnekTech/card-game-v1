using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class StatsUI : HBoxContainer
{
    [Node]
    private HBoxContainer blockContainer = null!;
    [Node]
    private Label blockLabel = null!;
    [Node]
    private HealthUI healthUI = null!;

    public void UpdateStats(Stats stats)
    {
        blockLabel.Text = stats.Block.ToString();
        healthUI.UpdateStats(stats);
        
        blockContainer.Visible = stats.Block > 0;
        healthUI.Visible = stats.Health > 0;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}