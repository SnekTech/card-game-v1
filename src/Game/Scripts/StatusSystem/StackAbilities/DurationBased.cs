namespace CardGameV1.StatusSystem.StackAbilities;

public class DurationBased : IStackAbility
{
    public int Duration { get; set; }
    public int Stacks
    {
        get => 0;
        set { }
    }

    public bool IsExpired => Duration <= 0;

    public void StackUp(IStackAbility other)
    {
        Duration += other.Duration;
    }

    public void Consume()
    {
        Duration--;
    }
}