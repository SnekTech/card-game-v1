using System.Threading.Tasks;
using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class ManaUI : Panel
{
    [Export]
    private CharacterStats originalStats = null!;

    [Node]
    private Label manaLabel = null!;

    private bool _hasSubscribedStatsChanged;
    private CharacterStats _stats = null!;

    public CharacterStats Stats
    {
        get => _stats;
        set
        {
            _stats = value;
            SubscribeStatsChanged();
        }
    }

    public override void _Ready()
    {
        Stats = originalStats;
        Test().Fire();

        async Task Test()
        {
            await this.DelayGd(1);
            Stats.Mana = 2;
        }
    }

    private void SubscribeStatsChanged()
    {
        if (_hasSubscribedStatsChanged == false)
        {
            Stats.StatsChanged += UpdateManaLabel;
        }

        _hasSubscribedStatsChanged = true;
    }

    private void UpdateManaLabel()
    {
        var (mana, maxMana) = (Stats.Mana, Stats.MaxMana);
        manaLabel.Text = $"{mana}/{maxMana}";
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}