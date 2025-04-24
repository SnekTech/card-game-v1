using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class Card : Resource
{
    [ExportGroup("Card Attributes")]
    [Export]
    public string Id { get; private set; } = "default";

    [Export]
    public CardType Type { get; private set; }

    [Export]
    public TargetType Target { get; private set; }

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