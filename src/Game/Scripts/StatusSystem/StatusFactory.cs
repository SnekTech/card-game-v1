using CardGameV1.StatusSystem.BuiltinStatuses;

namespace CardGameV1.StatusSystem;

public static class StatusFactory
{
    public static Exposed Exposed => new()
    {
        Id = nameof(Exposed),
        Type = StatusType.StartOfTurn,
        StackType = StackType.Duration,
        CanExpire = true,
        Duration = 1,
        Stacks = 0,
        IconPath = "res://art/expose.png",
    };

    public static Muscle Muscle => new()
    {
        Id = nameof(Muscle),
        Type = StatusType.EventBased,
        StackType = StackType.Intensity,
        CanExpire = false,
        Duration = 0,
        Stacks = 2,
        IconPath = "res://art/muscle.png",
    };

    public static TrueStrengthForm TrueStrengthForm => new()
    {
        Id = nameof(TrueStrengthForm),
        Type = StatusType.StartOfTurn,
        StackType = StackType.None,
        CanExpire = false,
        Duration = 0,
        Stacks = 0,
        IconPath = "res://art/tile_0127.png",
    };
}