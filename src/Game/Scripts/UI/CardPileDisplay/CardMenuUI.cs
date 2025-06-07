using System;
using CardGameV1.CardVisual;
using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.CardPileDisplay;

[Scene]
public partial class CardMenuUI : CenterContainer
{
    public event Action<Card>? TooltipRequested;

    [Export]
    private Card defaultCard = null!;

    [Node]
    private CardVisuals visuals = null!;

    private static readonly StyleBoxFlat BaseStyle =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_base_stylebox.tres");
    private static readonly StyleBoxFlat HoverStyle =
        GD.Load<StyleBoxFlat>("res://Scenes/CardVisual/card_hover_stylebox.tres");

    private Card? _card;

    public Card Card
    {
        get => _card ?? defaultCard;
        set
        {
            _card = value;
            visuals.Card = _card;
        }
    }

    public override void _Ready()
    {
        Card = defaultCard;
    }

    public override void _EnterTree()
    {
        visuals.MouseEntered += OnVisualsMouseEntered;
        visuals.MouseExited += OnVisualsMouseExited;
        visuals.GuiInput += OnVisualsGuiInput;
    }

    public override void _ExitTree()
    {
        visuals.MouseEntered -= OnVisualsMouseEntered;
        visuals.MouseExited -= OnVisualsMouseExited;
        visuals.GuiInput -= OnVisualsGuiInput;
    }

    private void OnVisualsGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            TooltipRequested?.Invoke(Card);
        }
    }

    private void OnVisualsMouseEntered() => visuals.Panel.SetStyleBox(HoverStyle);

    private void OnVisualsMouseExited() => visuals.Panel.SetStyleBox(BaseStyle);

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}