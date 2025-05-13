using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class ReleasedState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    private bool _played;
    
    public override void OnEnter()
    {
        _played = false;

        if (CardUI.Targets.Count > 0)
        {
            _played = true;
            CardUI.Play();
        }
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (_played)
            return;

        ChangeState<BaseState>();
    }
}