using System;
using System.Collections.Generic;
using CardGameV1.StatusSystem.BuiltinStatuses;

namespace CardGameV1.StatusSystem;

public static class StatusFactory
{
    private static readonly Dictionary<Type, Func<Status>> StatusGetters = new()
    {
        [typeof(Exposed)] = () => new Exposed
        {
            Id = nameof(Exposed),
            Type = StatusType.StartOfTurn,
            StackType = StackType.Duration,
            CanExpire = true,
            Duration = 1,
            Stacks = 0,
            IconPath = "res://art/expose.png",
        },
        [typeof(Muscle)] = () => new Muscle
        {
            Id = nameof(Muscle),
            Type = StatusType.EventBased,
            StackType = StackType.Intensity,
            CanExpire = false,
            Duration = 0,
            Stacks = 2,
            IconPath = "res://art/muscle.png",
        },
        [typeof(TrueStrengthForm)] = () => new TrueStrengthForm
        {
            Id = nameof(TrueStrengthForm),
            Type = StatusType.StartOfTurn,
            StackType = StackType.None,
            CanExpire = false,
            Duration = 0,
            Stacks = 0,
            IconPath = "res://art/tile_0127.png",
        },
    };

    public static T Create<T>() where T : Status
    {
        var getter = StatusGetters[typeof(T)];
        return (T)getter();
    }
}