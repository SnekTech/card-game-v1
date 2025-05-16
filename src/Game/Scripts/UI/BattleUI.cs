using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class BattleUI : CanvasLayer
{
    [Node]
    private Hand hand = null!;

    [Node]
    private ManaUI manaUI = null!;

    private CharacterStats _characterStats = null!;

    public CharacterStats CharacterStats
    {
        get => _characterStats;
        set
        {
            _characterStats = value;
            manaUI.CharacterStats = _characterStats;
            hand.CharacterStats = _characterStats;
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}