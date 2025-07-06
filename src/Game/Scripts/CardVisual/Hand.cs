using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;
using CardGameV1.ModifierSystem;
using GodotUtilities;

namespace CardGameV1.CardVisual;

public partial class Hand : HBoxContainer
{
    public CharacterStats CharacterStats { get; set; } = null!;

    public override void _ExitTree()
    {
        foreach (var cardUI in this.GetChildrenOfType<CardUI>())
        {
            cardUI.ReparentRequested -= OnCardUIReparentRequested;
        }
    }

    public void AddCard(Card card, ModifierHandler playerModifierHandler)
    {
        var newCardUI = SceneFactory.Instantiate<CardUI>();
        AddChild(newCardUI);

        newCardUI.ReparentRequested += OnCardUIReparentRequested;
        newCardUI.Card = card;
        newCardUI.Parent = this;
        newCardUI.CharacterStats = CharacterStats;
        newCardUI.PlayerModifierHandler = playerModifierHandler;
    }

    public void DiscardCard(CardUI card)
    {
        card.QueueFree();
    }

    public void DisableHand()
    {
        foreach (var cardUI in this.GetChildrenOfType<CardUI>())
        {
            cardUI.Disabled = true;
        }
    }

    private void OnCardUIReparentRequested(CardUI cardUI)
    {
        cardUI.Disabled = true;
        cardUI.Reparent(this);
        var newIndex = Mathf.Clamp(cardUI.OriginalIndex, 0, GetChildCount());

        Callable.From(DeferredWork).CallDeferred();
        return;

        void DeferredWork()
        {
            MoveChild(cardUI, newIndex);
            cardUI.Disabled = false;
        }
    }
}