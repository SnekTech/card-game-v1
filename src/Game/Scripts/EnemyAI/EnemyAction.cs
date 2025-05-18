using CardGameV1.Character;
using Godot;

namespace CardGameV1.EnemyAI;

public abstract class EnemyAction
{
    public Enemy? Enemy { get; set; }
    public Node2D? Target { get; set; }

    public abstract void PerformAction();
}

public abstract class EnemyConditionalAction : EnemyAction
{
    public virtual bool IsPerformable() => false;
}

public abstract class EnemyChanceBasedAction : EnemyAction
{
    public float ChanceWeight { get; set; }
    public float AccumulatedWeight { get; set; }
}