using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.CardVisual;

public partial class Hand : HBoxContainer
{
    [Export]
    private PackedScene cardUIScene = null!;

    public int CardsPlayedThisTurn { get; private set; }

    public CharacterStats CharacterStats { get; set; } = null!;

    public override void _Ready()
    {
        EventBus.EventBusOwner.CardEventBus.CardPlayed += OnCardPlayed;
    }

    public void AddCard(Card card)
    {
        var newCardUI = cardUIScene.Instantiate<CardUI>();
        AddChild(newCardUI);

        newCardUI.ReparentRequested += OnCardUIReparentRequested;
        newCardUI.Card = card;
        newCardUI.Parent = this;
        newCardUI.CharacterStats = CharacterStats;
    }

    private void OnCardPlayed(Card card)
    {
        CardsPlayedThisTurn++;
    }

    private void OnCardUIReparentRequested(CardUI cardUI)
    {
        cardUI.Disabled = true;
        cardUI.Reparent(this);
        var newIndex = Mathf.Clamp(cardUI.OriginalIndex - CardsPlayedThisTurn, 0, GetChildCount());

        Callable.From(DeferredWork).CallDeferred();
        return;

        void DeferredWork()
        {
            MoveChild(cardUI, newIndex);
            cardUI.Disabled = false;
        }
    }
}