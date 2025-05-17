using System.Threading.Tasks;
using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using Godot;

namespace CardGameV1.TurnManagement;

public partial class PlayerHandler : Node
{
    private const float HandDrawInterval = 0.25f;
    private const float HandDiscardInterval = 0.25f;

    [Export]
    private Hand hand = null!;

    private readonly PlayerEventBus playerEventBus = EventBusOwner.PlayerEventBus;
    private readonly CardEventBus cardEventBus = EventBusOwner.CardEventBus;
    private CharacterStats _characterStats = null!;

    public override void _Ready()
    {
        cardEventBus.CardPlayed += OnCardPlayed;
    }

    public void StartBattle(CharacterStats stats)
    {
        _characterStats = stats;
        _characterStats.DrawPile = (CardPile)stats.Deck.Duplicate(true);
        _characterStats.DrawPile.Shuffle();
        _characterStats.DiscardPile = new CardPile();
        StartTurn();
    }

    public void StartTurn()
    {
        _characterStats.Block = 0;
        _characterStats.ResetMana();
        DrawCardsAsync(_characterStats.CardsPerTurn).Fire();
    }

    public void EndTurn()
    {
        hand.DisableHand();
        DiscardCardsAsync().Fire();
    }

    private void DrawCard()
    {
        ReshuffleDeckFromDiscard();
        hand.AddCard(_characterStats.DrawPile.DrawCard());
        ReshuffleDeckFromDiscard();
    }

    private async Task DrawCardsAsync(int cardsPerTurn)
    {
        for (var i = 0; i < cardsPerTurn; i++)
        {
            DrawCard();
            await this.DelayGd(HandDrawInterval);
        }

        playerEventBus.EmitPlayerHandDrawn();
    }

    private async Task DiscardCardsAsync()
    {
        foreach (var child in hand.GetChildren())
        {
            if (child is CardUI cardUI)
            {
                _characterStats.DiscardPile.AddCard(cardUI.Card);
                hand.DiscardCard(cardUI);
                await this.DelayGd(HandDiscardInterval);
            }
        }
        
        playerEventBus.EmitPlayerHandDiscarded();
    }

    private void ReshuffleDeckFromDiscard()
    {
        if (_characterStats.DrawPile.IsEmpty == false)
            return;

        while (_characterStats.DiscardPile.IsEmpty == false)
        {
            _characterStats.DrawPile.AddCard(_characterStats.DiscardPile.DrawCard());
        }
        
        _characterStats.DrawPile.Shuffle();
    }

    private void OnCardPlayed(Card card)
    {
        _characterStats.DiscardPile.AddCard(card);
    }
}