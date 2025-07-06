using CardGameV1.Constants;
using CardGameV1.CustomResources.Cards;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileDisplay;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class CardTooltipPopup : Control
{
    [Export]
    private Color backgroundColor = new("000000b0");
    
    [Node]
    private ColorRect background = null!;
    [Node]
    private CenterContainer tooltipCard = null!;
    [Node]
    private RichTextLabel cardDescription = null!;

    public override void _Ready()
    {
        tooltipCard.ClearChildren();

        background.Color = backgroundColor;
    }

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputActions.LeftMouse))
        {
            HideTooltip();
        }
    }

    public void ShowTooltip(Card card)
    {
        var newCard = SceneFactory.Instantiate<CardMenuUI>();
        tooltipCard.AddChild(newCard);
        newCard.Card = card;
        newCard.TooltipRequested += OnCardTooltipRequested;
        cardDescription.Text = card.TooltipText;
        Show();
    }

    public void HideTooltip()
    {
        if (Visible == false)
            return;

        foreach (var child in tooltipCard.GetChildrenOfType<CardMenuUI>())
        {
            child.TooltipRequested -= OnCardTooltipRequested;
            child.QueueFree();
        }

        Hide();
    }

    private void OnCardTooltipRequested(Card _) => HideTooltip();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}