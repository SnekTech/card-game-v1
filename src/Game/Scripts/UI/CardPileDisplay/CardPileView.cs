using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.MyExtensions;
using GodotUtilities;

namespace CardGameV1.UI.CardPileDisplay;

[Scene]
public partial class CardPileView : Control
{
    [Node]
    private Label title = null!;
    [Node]
    private GridContainer cards = null!;
    [Node]
    private Button backButton = null!;
    [Node]
    private CardTooltipPopup cardTooltipPopup = null!;

    private CardPile? _cardPile;
    
    public CardPile CardPile
    {
        set => _cardPile = value;
    }

    public override void _Ready()
    {
        cards.ClearChildren();
        cardTooltipPopup.HideTooltip();
    }

    public override void _EnterTree()
    {
        backButton.Pressed += Hide;
    }

    public override void _ExitTree()
    {
        backButton.Pressed -= Hide;
        foreach (var card in cards.GetChildrenOfType<CardMenuUI>())
        {
            card.TooltipRequested -= cardTooltipPopup.ShowTooltip;
        }
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(BuiltinInputActions.UICancel))
        {
            if (cardTooltipPopup.Visible)
            {
                cardTooltipPopup.HideTooltip();
            }
            else
            {
                Hide();
            }
        }
    }

    public void ShowCurrentView(string newTitle, bool randomized = false)
    {
        cards.ClearChildren();

        cardTooltipPopup.HideTooltip();
        title.Text = newTitle;
        Callable.From(() => UpdateView(randomized)).CallDeferred();
    }

    private void UpdateView(bool randomized)
    {
        if (_cardPile == null)
            return;
        
        var allCards = _cardPile.Cards;
        if (randomized)
        {
            allCards.Shuffle();
        }

        foreach (var card in allCards)
        {
            var newCard = SceneFactory.Instantiate<CardMenuUI>();
            cards.AddChild(newCard);
            newCard.Card = card;
            newCard.TooltipRequested += cardTooltipPopup.ShowTooltip;
        }

        Show();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}