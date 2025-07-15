using System;
using System.Collections.Generic;
using CardGameV1.CustomResources.Cards.CardTargetGetters;
using CardGameV1.CustomResources.Cards.Warrior;

namespace CardGameV1.CustomResources.Cards;

public static class CardFactory
{
    private static readonly Dictionary<Type, Func<Card>> CardGetters = new()
    {
        [typeof(WarriorAxeAttack)] = () => new WarriorAxeAttack { TargetGetter = new SingleEnemy() },
        [typeof(WarriorBigSlam)] = () => new WarriorBigSlam { TargetGetter = new SingleEnemy() },
        [typeof(WarriorBlock)] = () => new WarriorBlock { TargetGetter = new Self() },
        [typeof(WarriorSlash)] = () => new WarriorSlash { TargetGetter = new AllEnemies() },
        [typeof(WarriorTrueStrength)] = () => new WarriorTrueStrength { TargetGetter = new Self() },
    };

    public static T Create<T>() where T : Card => (T)CardGetters[typeof(T)]();
}