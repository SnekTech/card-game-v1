using System;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.StatusSystem;

public class Status
{
    public event Action? Changed;

    public string Id { get; init; } = "default status";
    public StatusType Type { get; init; }
    public StackType StackType { get; init; }
    public bool CanExpire { get; init; }

    public required string IconPath { get; init; }
    public Texture2D Icon => SnekUtility.LoadTexture(IconPath);
    public string Tooltip { get; init; } = "default tooltip for status";

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

    public void Init(ITarget target)
    {
        GD.Print($"init status {Id}");
    }

    public Task ApplyStatusAsync(ITarget target)
    {
        GD.Print($"applying status {Id}");
        return Task.CompletedTask;
    }
}