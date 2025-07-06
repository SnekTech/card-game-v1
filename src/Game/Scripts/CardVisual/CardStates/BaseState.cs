using CardGameV1.Constants;

namespace CardGameV1.CardVisual.CardStates;

public class BaseState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    public override void OnEnter()
    {
        CardUI.StopAnimation();

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        CardUI.EmitReparentRequested();
        CardUI.PivotOffset = Vector2.Zero;
        CardEvents.EmitTooltipHideRequested();
    }

    public override void OnGuiInput(InputEvent inputEvent)
    {
        if (CardUI.Playable == false || CardUI.Disabled)
            return;

        if (inputEvent.IsActionPressed(InputActions.LeftMouse))
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
        CardUI.RequestTooltip();
    }

    public override void OnMouseExited()
    {
        if (CardUI.Playable == false || CardUI.Disabled)
            return;

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        CardEvents.EmitTooltipHideRequested();
    }
}