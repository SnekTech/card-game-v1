using CardGameV1.Constants;
using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class BaseState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    public override void OnEnter()
    {
        CardUI.StopAnimation();

        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
        CardUI.EmitReparentRequested();
        CardUI.PivotOffset = Vector2.Zero;
    }

    public override void OnGuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputActionNames.LeftMouse))
        {
            CardUI.PivotOffset = CardUI.GetGlobalMousePosition() - CardUI.GlobalPosition;
            ChangeState<ClickedState>();
        }
    }

    public override void OnMouseEntered()
    {
        CardUI.SetPanelStyleBox(CardUI.HoverStyleBox);
    }

    public override void OnMouseExited()
    {
        CardUI.SetPanelStyleBox(CardUI.BaseStyleBox);
    }
}