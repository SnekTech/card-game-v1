using CardGameV1.Constants;
using CardGameV1.EventBus;
using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class BaseState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    private static readonly CardEventBus CardEventBus = EventBusOwner.CardEventBus;
    
    public override void OnEnter()
    {
        CardUI.StopAnimation();

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        CardUI.EmitReparentRequested();
        CardUI.PivotOffset = Vector2.Zero;
        CardEventBus.EmitTooltipHideRequested();
    }

    public override void OnGuiInput(InputEvent inputEvent)
    {
        if (CardUI.Playable == false || CardUI.Disabled)
            return;

        if (inputEvent.IsActionPressed(InputActionNames.LeftMouse))
        {
            CardUI.PivotOffset = CardUI.GetGlobalMousePosition() - CardUI.GlobalPosition;
            ChangeState<ClickedState>();
        }
    }

    public override void OnMouseEntered()
    {
        if (CardUI.Playable == false || CardUI.Disabled)
            return;

        CardUI.SetPanelStyleBox(CardUI.HoverStyleBox);
        var icon = CardUI.Card.Icon;
        var tooltipText = CardUI.Card.TooltipText;
        CardEventBus.EmitCardTooltipRequested(icon, tooltipText);
    }

    public override void OnMouseExited()
    {
        if (CardUI.Playable == false || CardUI.Disabled)
            return;

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        CardEventBus.EmitTooltipHideRequested();
    }
}