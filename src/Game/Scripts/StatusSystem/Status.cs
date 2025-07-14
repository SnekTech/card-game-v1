using CardGameV1.EffectSystem;

namespace CardGameV1.StatusSystem;

public abstract class Status
{
    public event Action? Changed;

    public string Id { get; init; } = "default status";
    public required StatusType Type { get; init; }
    public required string IconPath { get; init; }
    public Texture2D Icon => SnekUtility.LoadTexture(IconPath);
    public abstract string Tooltip { get; }

    public required IStackAbility StackAbility { get; init; }
    public int Duration => StackAbility.Duration;
    public int Stacks => StackAbility.Stacks;
    public bool IsExpired => StackAbility.IsExpired;

    public void SetDuration(int duration)
    {
        StackAbility.Duration = duration;
        EmitChanged();
    }

    public void SetStacks(int stacks)
    {
        StackAbility.Stacks = stacks;
        EmitChanged();
    }

    public virtual void Init(ITarget target)
    {
        GD.Print($"init status {Id} for target {target}");
    }

    public void StackUp(Status other)
    {
        StackAbility.StackUp(other.StackAbility);
        EmitChanged();
    }

    public void Consume()
    {
        StackAbility.Consume();
        EmitChanged();
    }

    public abstract Task ApplyStatusAsync(ITarget target, CancellationToken cancellationToken);

    private void EmitChanged() => Changed?.Invoke();
}