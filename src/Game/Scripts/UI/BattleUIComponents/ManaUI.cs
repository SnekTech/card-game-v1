using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class ManaUI : Panel
{
    [Node]
    private Label manaLabel = null!;

    private CharacterStats _characterStats = null!;

    public CharacterStats CharacterStats
    {
        get => _characterStats;
        set
        {
            _characterStats = value;
            _characterStats.StatsChanged += UpdateManaLabel;
        }
    }

    public override void _ExitTree()
    {
        _characterStats.StatsChanged -= UpdateManaLabel;
    }

    private void UpdateManaLabel()
    {
        var (mana, maxMana) = (CharacterStats.Mana, CharacterStats.MaxMana);
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