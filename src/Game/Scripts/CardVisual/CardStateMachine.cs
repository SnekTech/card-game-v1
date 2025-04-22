using CardGameV1.CardVisual.CardStates;
using CardGameV1.FSM;
using Godot;

namespace CardGameV1.CardVisual;

public class CardStateMachine(CardUI cardUI) : StateMachine<CardState>
{
    public readonly CardUI CardUI = cardUI;

    protected override void InstantiateStateInstances()
    {
        StateInstances[typeof(BaseState)] = new BaseState(this);
        StateInstances[typeof(ClickedState)] = new ClickedState(this);
        StateInstances[typeof(DraggingState)] = new DraggingState(this);
        StateInstances[typeof(ReleasedState)] = new ReleasedState(this);
        StateInstances[typeof(AimingState)] = new AimingState(this);
    }

    public void OnInput(InputEvent inputEvent) => CurrentState?.OnInput(inputEvent);
    public void OnGuiInput(InputEvent inputEvent) => CurrentState?.OnGuiInput(inputEvent);
    public void OnMouseEntered() => CurrentState?.OnMouseEntered();
    public void OnMouseExited() => CurrentState?.OnMouseExited();
}