using CardGameV1.EnemyAI;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class IntentUI : HBoxContainer
{
    [Node]
    private TextureRect icon = null!;

    [Node]
    private Label numberLabel = null!;

    public void UpdateIntent(Intent? intent)
    {
        if (intent == null)
        {
            Hide();
            return;
        }

        icon.Texture = intent.Icon;
        numberLabel.Text = intent.Number;
        Show();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}