namespace CardGameV1.StatusSystem.StackAbilities;

public class NonStackable : IStackAbility
{
    public int Duration
    {
        get => 0;
        set { }
    }
    public int Stacks
    {
        get => 0;
        set { }
    }

    public bool IsExpired => false;

    public void StackUp(IStackAbility other)
    {
    }

    public void Consume()
    {
    }
}