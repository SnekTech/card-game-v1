﻿using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class Card : Resource
{
    [Export] public string Id { get; private set; } = "default";

    [Export] public int Cost { get; private set; } = 1;

    [Export] public CardType Type { get; private set; }

    [Export] public TargetType Target { get; private set; }

    public bool IsSingleTargeted => Target == TargetType.SingleEnemy;

    #region card enums

    public enum CardType
    {
        Attack,
        Skill,
        Power
    }

    public enum TargetType
    {
        Self,
        SingleEnemy,
        AllEnemies,
        Everyone
    }

    #endregion
}