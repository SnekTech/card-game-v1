using System.Threading.Tasks;
using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.TurnManagement;

public partial class PlayerHandler : Node
{
    private const float HandDrawInterval = 0.25f;

    [Export]
    private Hand hand = null!;

    private CharacterStats _characterStats = null!;

    public void StartBattle(CharacterStats stats)
    {
        _characterStats = stats;
        _characterStats.DrawPile = (CardPile)stats.Deck.Duplicate(true);
        _characterStats.DrawPile.Shuffle();
        _characterStats.DiscardPile = new CardPile();
        StartTurn();
    }

    private void StartTurn()
    {
        _characterStats.Block = 0;
        _characterStats.ResetMana();
        DrawCardsAsync(_characterStats.CardsPerTurn).Fire();
    }

    private void DrawCard()
    {
        hand.AddCard(_characterStats.DrawPile.DrawCard());
    }

    private async Task DrawCardsAsync(int cardsPerTurn)
    {
        for (var i = 0; i < cardsPerTurn; i++)
        {
            DrawCard();
            await this.DelayGd(HandDrawInterval);
        }

        EventBus.EventBusOwner.PlayerEventBus.EmitPlayerHandDrawn();
    }
}