using CardGameV1.WeightedRandom;

namespace CardGameV1.EnemyAI;

public class EnemyChanceBasedAction : EnemyAction, IWeightedCandidate
{
    public required float Weight { get; init; }
}