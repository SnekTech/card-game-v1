using CardGameV1.StatusSystem.BuiltinStatuses;
using CardGameV1.StatusSystem.StackAbilities;

namespace CardGameV1.StatusSystem;

public static class StatusFactory
{
    public static Exposed CreateExposed(int duration)
    {
        var exposed = new Exposed
        {
            Id = nameof(Exposed),
            Type = StatusType.StartOfTurn,
            IconPath = "res://art/expose.png",
            StackAbility = new DurationBased(),
        };
        exposed.SetDuration(duration);
        return exposed;
    }

    public static Muscle CreateMuscle(int stacks)
    {
        var muscle = new Muscle
        {
            Id = nameof(Muscle),
            Type = StatusType.EventBased,
            IconPath = "res://art/muscle.png",
            StackAbility = new IntensityBased(),
        };
        muscle.SetStacks(stacks);
        return muscle;
    }

    public static TrueStrengthForm CreateTrueStrengthForm() =>
        new()
        {
            Id = nameof(TrueStrengthForm),
            Type = StatusType.StartOfTurn,
            IconPath = "res://art/tile_0127.png",
            StackAbility = new NonStackable(),
        };
}