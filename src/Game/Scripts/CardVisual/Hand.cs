using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.CardVisual;

public partial class Hand : HBoxContainer
{
    public int CardsPlayedThisTurn { get; private set; }

    public override void _Ready()
    {
        EventBus.EventBusOwner.CardEventBus.CardPlayed += OnCardPlayed;

        foreach (var child in GetChildren())
        {
            if (child is not CardUI cardUI) continue;

            cardUI.ReparentRequested += OnCardUIReparentRequested;
            cardUI.Parent = this;
        }
    }

    private void OnCardPlayed(Card card)
    {
        CardsPlayedThisTurn++;
    }

    private void OnCardUIReparentRequested(CardUI cardUI)
    {
        cardUI.Reparent(this);
        var newIndex = Mathf.Clamp(cardUI.OriginalIndex - CardsPlayedThisTurn, 0, GetChildCount());
        Callable.From(() => MoveChild(cardUI, newIndex)).CallDeferred();
    }
}