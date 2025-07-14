namespace CardGameV1.StatusSystem;

public interface IStackAbility
{
    public int Duration { get; set; }
    public int Stacks { get; set; }
    public bool IsExpired { get; }
    public void StackUp(IStackAbility other);
    public void Consume();
}