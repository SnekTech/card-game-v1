using System.Threading;
using System.Threading.Tasks;
using CardGameV1.CardVisual;
using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;
using CardGameV1.EventBus;
using CardGameV1.StatusSystem;
using Godot;
using GodotUtilities;

namespace CardGameV1.TurnManagement;

public partial class PlayerHandler : Node
{
    [Export]
    private Player player = null!;
    [Export]
    private Hand hand = null!;

    private const float HandDrawInterval = 0.25f;
    private const float HandDiscardInterval = 0.25f;

    private static readonly PlayerEvents PlayerEvents = EventBusOwner.PlayerEvents;
    private static readonly CardEvents CardEvents = EventBusOwner.CardEvents;
    private CharacterStats _characterStats = null!;

    public override void _EnterTree()
    {
        CardEvents.CardPlayed += OnCardPlayed;
    }

    public override void _ExitTree()
    {
        CardEvents.CardPlayed -= OnCardPlayed;
    }

    public void StartBattle(CharacterStats stats)
    {
        _characterStats = stats;
        _characterStats.DrawPile = new CardPile(stats.Deck.Cards);
        _characterStats.DrawPile.Shuffle();
        _characterStats.DiscardPile = new CardPile();
        StartTurn();
    }

    public void StartTurn()
    {
        _characterStats.Block = 0;
        _characterStats.ResetMana();
        StartTurnAsync().Fire();
    }

    public void EndTurn()
    {
        hand.DisableHand();
        EndTurnAsync().Fire();
    }

    private void DrawCard()
    {
        ReshuffleDeckFromDiscard();

        var card = _characterStats.DrawPile.DrawCard();
        if (card == null)
        {
            return;
        }

        hand.AddCard(card);
        ReshuffleDeckFromDiscard();
    }

    private async Task DrawCardsAsync(int cardsPerTurn)
    {
        for (var i = 0; i < cardsPerTurn; i++)
        {
            DrawCard();
            await this.DelayGd(HandDrawInterval);
        }

        PlayerEvents.EmitPlayerHandDrawn();
    }

    private async Task DiscardCardsAsync()
    {
        foreach (var cardUI in hand.GetChildrenOfType<CardUI>())
        {
            _characterStats.DiscardPile.AddCard(cardUI.Card);
            hand.DiscardCard(cardUI);
            await this.DelayGd(HandDiscardInterval);
        }

        PlayerEvents.EmitPlayerHandDiscarded();
    }

    private async Task StartTurnAsync()
    {
        // todo: design the task cancel flow
        await player.StatusHandler.ApplyStatusesByType(StatusType.StartOfTurn, player.CancellationTokenOnQueueFree);
        await DrawCardsAsync(_characterStats.CardsPerTurn);
    }

    private async Task EndTurnAsync()
    {
        await player.StatusHandler.ApplyStatusesByType(StatusType.EndOfTurn, player.CancellationTokenOnQueueFree);
        await DiscardCardsAsync();
    }

    private void ReshuffleDeckFromDiscard()
    {
        if (!_characterStats.DrawPile.IsEmpty)
            return;

        while (!_characterStats.DiscardPile.IsEmpty)
        {
            _characterStats.DrawPile.AddCard(_characterStats.DiscardPile.DrawCard()!);
        }

        _characterStats.DrawPile.Shuffle();
    }

    private void OnCardPlayed(Card card)
    {
        _characterStats.DiscardPile.AddCard(card);
    }
}