using CardGameV1.CustomResources.Run;

namespace CardGameV1.UI.Run;

[SceneTree]
public partial class GoldUI : HBoxContainer
{
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

    private void UpdateContent() => _.Label.Text = RunStats.Gold.ToString();

    private void OnGoldChanged() => UpdateContent();
}