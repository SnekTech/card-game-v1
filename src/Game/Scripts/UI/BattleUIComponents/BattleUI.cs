using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.UI.CardPileDisplay;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class BattleUI : CanvasLayer
{
    [Export]
    private CardPileView drawPileView = null!;
    [Export]
    private CardPileView discardPileView = null!;

    [Node]
    private Hand hand = null!;
    [Node]
    private ManaUI manaUI = null!;
    [Node]
    private Button endTurnButton = null!;
    [Node]
    private CardPileOpener drawPileButton = null!;
    [Node]
    private CardPileOpener discardPileButton = null!;

    private CharacterStats _characterStats = null!;
    private static readonly PlayerEvents PlayerEvents = EventBusOwner.PlayerEvents;

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

    public override void _EnterTree()
    {
        PlayerEvents.PlayerHandDrawn += OnPlayerHandDrawn;
        endTurnButton.Pressed += OnEndTurnButtonPressed;
        drawPileButton.Pressed += OnDrawPileButtonPressed;
        discardPileButton.Pressed += OnDiscardPileButtonPressed;
    }

    public override void _ExitTree()
    {
        PlayerEvents.PlayerHandDrawn -= OnPlayerHandDrawn;
        endTurnButton.Pressed -= OnEndTurnButtonPressed;
        drawPileButton.Pressed -= OnDrawPileButtonPressed;
        discardPileButton.Pressed -= OnDiscardPileButtonPressed;
    }

    public void InitCardPileUI()
    {
        drawPileButton.CardPile = CharacterStats.DrawPile;
        drawPileView.CardPile = CharacterStats.DrawPile;
        discardPileButton.CardPile = CharacterStats.DiscardPile;
        discardPileView.CardPile = CharacterStats.DiscardPile;
    }

    private void OnPlayerHandDrawn()
    {
        endTurnButton.Disabled = false;
    }

    private void OnEndTurnButtonPressed()
    {
        endTurnButton.Disabled = true;
        PlayerEvents.EmitPlayerTurnEnded();
    }

    private void OnDrawPileButtonPressed() => drawPileView.ShowCurrentView("Draw Pile", true);
    private void OnDiscardPileButtonPressed() => discardPileView.ShowCurrentView("Discard Pile");

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}