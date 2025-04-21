namespace CardGameV1.FSM;

public interface IState
{
    void OnEnter();
    void OnExit();
}