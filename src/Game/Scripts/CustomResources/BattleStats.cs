using CardGameV1.WeightedRandom;

namespace CardGameV1.CustomResources;

public record BattleStats : IWeightedCandidate
{
    public required int Tier { get; init; }
    public required (int min, int max) GoldReward { get; init; }
    public required List<EnemyStats> Enemies { get; init; }
    public int GoldRewardRoll => GD.RandRange(GoldReward.min, GoldReward.max);
    public required float Weight { get; init; }
}