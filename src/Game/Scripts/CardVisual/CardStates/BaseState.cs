using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class BaseState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    public override void OnEnter()
    {
        CardUI.EmitReparentRequested();
        CardUI.SetDebugInfo(Colors.WebGreen, nameof(BaseState));
        CardUI.PivotOffset = Vector2.Zero;
    }

    public override void OnGuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("left_mouse"))
        {
            CardUI.PivotOffset = CardUI.GetGlobalMousePosition() - CardUI.GlobalPosition;
            ChangeState<ClickedState>();
        }
    }
}