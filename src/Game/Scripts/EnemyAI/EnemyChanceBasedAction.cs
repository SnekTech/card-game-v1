using Godot;

namespace CardGameV1.EnemyAI;

public abstract partial class EnemyChanceBasedAction : EnemyAction
{
    [Export]
    public float ChanceWeight { get; set; }

    public float AccumulatedWeight { get; set; }
}