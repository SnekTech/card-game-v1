using Godot;

namespace CardGameV1.CardUI.CardStates;

public class ClickedState(CardDragStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    public override void OnEnter()
    {
        CardUI.SetDebugInfo(Colors.Orange, nameof(ClickedState));
        CardUI.MonitoringDrop = true;
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseMotion)
        {
            ChangeState<DraggingState>();
        }
    }
}