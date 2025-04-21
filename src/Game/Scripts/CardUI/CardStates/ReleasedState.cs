using Godot;

namespace CardGameV1.CardUI.CardStates;

public class ReleasedState(CardDragStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    private bool _played;
    
    public override void OnEnter()
    {
        CardUI.SetDebugInfo(Colors.DarkViolet, nameof(ReleasedState));

        _played = false;

        if (CardUI.Targets.Count > 0)
        {
            _played = true;
            GD.Print($"play card for target(s) {CardUI.Targets}");
        }
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (_played)
            return;

        ChangeState<BaseState>();
    }
}