using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class ClickedState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    public override void OnEnter()
    {
        CardUI.MonitoringDrop = true;
        CardUI.OriginalIndex = CardUI.GetIndex();
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseMotion)
        {
            ChangeState<DraggingState>();
        }
    }
}