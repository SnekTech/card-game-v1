using CardGameV1.CustomResources;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileView;
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
        GD.Load<PackedScene>("res://Scenes/UI/card_pile_view/CardMenuUI.tscn");

    public override void _Ready()
    {
        tooltipCard.ClearChildren();
    }

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("left_mouse"))
        {
            HideTooltip();
        }
    }

    private void ShowTooltip(Card card)
    {
        var newCard = CardMenuUIScene.Instantiate<CardMenuUI>();
        tooltipCard.AddChild(newCard);
        newCard.Card = card;
        newCard.TooltipRequested += OnCardTooltipRequested;
        cardDescription.Text = card.TooltipText;
        Show();
    }

    private void HideTooltip()
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