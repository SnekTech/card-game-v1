using Godot;

namespace CardGameV1.EnemyAI;

public abstract class EnemyChanceBasedAction : EnemyAction
{
    public abstract float ChanceWeight { get; }

    public float AccumulatedWeight { get; set; }
}