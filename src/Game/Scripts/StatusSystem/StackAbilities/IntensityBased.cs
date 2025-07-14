namespace CardGameV1.StatusSystem.StackAbilities;

public class IntensityBased : IStackAbility
{
    public int Duration
    {
        get => 0;
        set { }
    }
    public int Stacks { get; set; }

    public bool IsExpired => Stacks == 0;

    public void StackUp(IStackAbility other)
    {
        Stacks += other.Stacks;
    }

    public void Consume()
    {
    }
}