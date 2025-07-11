namespace CardGameV1.EnemyAI;

public class EnemyChanceBasedAction : EnemyAction
{
    public required float ChanceWeight { get; init; }
    public float AccumulatedWeight { get; set; }
}