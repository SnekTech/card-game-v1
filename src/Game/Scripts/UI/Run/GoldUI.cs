using CardGameV1.CustomResources.Run;
using GodotUtilities;

namespace CardGameV1.UI.Run;

[Scene]
public partial class GoldUI : HBoxContainer
{
    [Node]
    private TextureRect icon = null!;
    [Node]
    private Label label = null!;

    private RunStats? _runStats;

    public RunStats RunStats
    {
        get => _runStats!;
        set
        {
            if (_runStats != null)
            {
                _runStats.GoldChanged -= OnGoldChanged;
            }

            _runStats = value;
            _runStats.GoldChanged += OnGoldChanged;
            UpdateContent();
        }
    }

    public override void _ExitTree()
    {
        RunStats.GoldChanged -= OnGoldChanged;
    }

    private void UpdateContent() => label.Text = RunStats.Gold.ToString();

    private void OnGoldChanged() => UpdateContent();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}