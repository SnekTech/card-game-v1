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

public class BattleStatsPool
{
    public static BattleStats TestBattle => new()
    {
        Tier = 0,
        Weight = 2.5f,
        GoldReward = (53, 72),
        EnemiesScenePath = "res://Scenes/battles/Tier0Bats2.tscn"
    };

    private void Init()
    {
        var tier0Bats2 = new BattleStats
        {
            Tier = 0,
            Weight = 2.5f,
            GoldReward = (53, 72),
            EnemiesScenePath = "res://Scenes/battles/Tier0Bats2.tscn"
        };
        var tier0Crab = new BattleStats
        {
            Tier = 0,
            Weight = 3f,
            GoldReward = (49, 68),
            EnemiesScenePath = "res://Scenes/battles/Tier0Crab.tscn"
        };
        var tier1Bats3 = new BattleStats
        {
            Tier = 1,
            Weight = 1f,
            GoldReward = (63, 81),
            EnemiesScenePath = "res://Scenes/battles/Tier1Bats3.tscn"
        };
        var tier1BatCrab = new BattleStats
        {
            Tier = 1,
            Weight = 2f,
            GoldReward = (63, 81),
            EnemiesScenePath = "res://Scenes/battles/Tier1BatCrab.tscn"
        };
    }
}