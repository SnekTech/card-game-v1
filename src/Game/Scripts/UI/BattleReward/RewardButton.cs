using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[Scene]
public partial class RewardButton : Button
{
    [Node]
    private TextureRect customIcon = null!;
    [Node]
    private Label customText = null!;

    public Texture2D RewardIcon
    {
        set => customIcon.Texture = value;
    }

    public string RewardText
    {
        set => customText.Text = value;
    }

    public override void _EnterTree()
    {
        Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        QueueFree();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}