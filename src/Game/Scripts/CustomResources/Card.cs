using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class Card : Resource
{
    [ExportGroup("Card Attributes")]
    [Export]
    private string id = "default";

    [Export] private CardType type;

    [Export] private TargetType target;

    public bool IsSingleTargeted => target == TargetType.SingleEnemy;

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