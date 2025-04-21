using Godot;

namespace CardGameV1.CardUI.CardStates;

public class BaseState(CardDragStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
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