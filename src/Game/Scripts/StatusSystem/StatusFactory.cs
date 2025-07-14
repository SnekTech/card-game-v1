using CardGameV1.StatusSystem.BuiltinStatuses;
using CardGameV1.StatusSystem.StackAbilities;

namespace CardGameV1.StatusSystem;

public static class StatusFactory
{
    private static readonly Dictionary<Type, Func<Status>> StatusGetters = new()
    {
        [typeof(Exposed)] = () => new Exposed
        {
            Id = nameof(Exposed),
            Type = StatusType.StartOfTurn,
            IconPath = "res://art/expose.png",
            StackAbility = new DurationBased(),
        },
        [typeof(Muscle)] = () => new Muscle
        {
            Id = nameof(Muscle),
            Type = StatusType.EventBased,
            IconPath = "res://art/muscle.png",
            StackAbility = new IntensityBased(),
        },
        [typeof(TrueStrengthForm)] = () => new TrueStrengthForm
        {
            Id = nameof(TrueStrengthForm),
            Type = StatusType.StartOfTurn,
            IconPath = "res://art/tile_0127.png",
            StackAbility = new NonStackable(),
        },
    };

    public static T Create<T>() where T : Status
    {
        var getter = StatusGetters[typeof(T)];
        return (T)getter();
    }
}