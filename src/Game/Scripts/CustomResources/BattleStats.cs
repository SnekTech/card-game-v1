using Godot;

namespace CardGameV1.CustomResources;

public record BattleStats
{
    public required int Tier { get; init; }
    public required float Weight { get; init; }
    public required (int min, int max) GoldReward { get; init; }
    public required string EnemiesScenePath { get; init; }

    public float AccumulatedWeight { get; set; }

    public int GoldRewardRoll => GD.RandRange(GoldReward.min, GoldReward.max);
}