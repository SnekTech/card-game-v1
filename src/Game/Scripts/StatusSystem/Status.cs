using System.Threading;
using CardGameV1.EffectSystem;

namespace CardGameV1.StatusSystem;

public abstract class Status
{
    public event Action? Changed;

    public string Id { get; init; } = "default status";
    public StatusType Type { get; init; }
    public StackType StackType { get; init; }
    public bool IsStackable => StackType != StackType.None;
    public bool CanExpire { get; init; }

    public required string IconPath { get; init; }
    public Texture2D Icon => SnekUtility.LoadTexture(IconPath);
    public abstract string Tooltip { get; }

    private int _duration;
    private int _stacks;

    public int Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            Changed?.Invoke();
        }
    }

    public int Stacks
    {
        get => _stacks;
        set
        {
            _stacks = value;
            Changed?.Invoke();
        }
    }

    public virtual void Init(ITarget target)
    {
        GD.Print($"init status {Id} for target {target}");
    }

    public virtual Task ApplyStatusAsync(ITarget target, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}