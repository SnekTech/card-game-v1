using CardGameV1.EnemyAI;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class IntentUI : HBoxContainer
{
    [Node]
    private TextureRect icon = null!;

    [Node]
    private Label label = null!;

    public void UpdateIntent(Intent? intent)
    {
        if (intent == null)
        {
            Hide();
            return;
        }

        icon.Texture = intent.Icon;
        label.Text = intent.CurrentText;
        label.Visible = intent.CurrentText.Length > 0;
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