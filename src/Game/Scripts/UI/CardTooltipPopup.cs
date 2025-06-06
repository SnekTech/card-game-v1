using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileDisplay;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class CardTooltipPopup : Control
{
    [Node]
    private CenterContainer tooltipCard = null!;
    [Node]
    private RichTextLabel cardDescription = null!;

    private static readonly PackedScene CardMenuUIScene =
        GD.Load<PackedScene>(ScenePath.CardMenuUI);

    public override void _Ready()
    {
        tooltipCard.ClearChildren();
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
        var newCard = CardMenuUIScene.Instantiate<CardMenuUI>();
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