using Godot;

namespace CardGameV1.CardUI.CardStates;

public class ReleasedState(CardDragStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    public override void OnEnter()
    {
        CardUI.SetDebugInfo(Colors.DarkViolet, nameof(ReleasedState));
    }
}