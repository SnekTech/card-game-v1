namespace CardGameV1.UI.BattleReward;

[SceneTree]
public partial class RewardButton : Button
{
    public Texture2D RewardIcon
    {
        set => CustomIcon.Texture = value;
    }

    public string RewardText
    {
        set => CustomText.Text = value;
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
}