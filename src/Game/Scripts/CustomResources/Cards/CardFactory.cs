using System;
using System.Collections.Generic;
using CardGameV1.CustomResources.Cards.Warrior;

namespace CardGameV1.CustomResources.Cards;

public static class CardFactory
{
    private static readonly Dictionary<Type, Func<Card>> CardGetters = new()
    {
        [typeof(WarriorAxeAttack)] = () => new WarriorAxeAttack(),
        [typeof(WarriorBigSlam)] = () => new WarriorBigSlam(),
        [typeof(WarriorBlock)] = () => new WarriorBlock(),
        [typeof(WarriorSlash)] = () => new WarriorSlash(),
        [typeof(WarriorTrueStrength)] = () => new WarriorTrueStrength(),
    };

    public static T Create<T>() where T : Card => (T)CardGetters[typeof(T)]();
}