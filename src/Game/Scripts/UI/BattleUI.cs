using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
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

    [Node]
    private Button endTurnButton = null!;

    private CharacterStats _characterStats = null!;
    private readonly PlayerEventBus playerEventBus = EventBusOwner.PlayerEventBus;

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

    public override void _Ready()
    {
        playerEventBus.PlayerHandDrawn += OnPlayerHandDrawn;
        endTurnButton.Pressed += OnEndTurnButtonPressed;
    }

    private void OnPlayerHandDrawn()
    {
        endTurnButton.Disabled = false;
    }

    private void OnEndTurnButtonPressed()
    {
        endTurnButton.Disabled = true;
        playerEventBus.EmitPlayerTurnEnded();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}