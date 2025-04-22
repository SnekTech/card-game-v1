using CardGameV1.FSM;
using Godot;

namespace CardGameV1.CardVisual;

public abstract class CardState(CardDragStateMachine cardDragStateMachine) : IState
{
    protected CardUI CardUI => cardDragStateMachine.CardUI;

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnInput(InputEvent inputEvent)
    {
    }

    public virtual void OnGuiInput(InputEvent inputEvent)
    {
    }

    public virtual void OnMouseEntered()
    {
    }

    public virtual void OnMouseExited()
    {
    }

    protected void ChangeState<T>() where T : CardState
    {
        cardDragStateMachine.ChangeState<T>();
    }
}