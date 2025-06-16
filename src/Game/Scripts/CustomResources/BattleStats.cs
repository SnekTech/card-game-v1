using System.Collections.Generic;
using System.Linq;
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

    private BattleStatsPool()
    {
    }

    private static BattleStatsPool? _instance;
    public static BattleStatsPool Instance => _instance ??= new BattleStatsPool();

    private readonly List<BattleStats> _pool = GenerateDefaultBattles();
    private readonly float[] _totalWeightsByTier = [0, 0, 0];

    private List<BattleStats> GetAllBattlesForTier(int tier) => _pool.Where(battle => battle.Tier == tier).ToList();

    public void SetupWeightForTier(int tier)
    {
        var battles = GetAllBattlesForTier(tier);
        _totalWeightsByTier[tier] = 0;

        foreach (var battle in battles)
        {
            _totalWeightsByTier[tier] += battle.Weight;
            battle.AccumulatedWeight = _totalWeightsByTier[tier];
        }
    }

    public BattleStats GetRandomBattleForTier(int tier)
    {
        var roll = GD.RandRange(0, _totalWeightsByTier[tier]);
        var battles = GetAllBattlesForTier(tier);

        var rolledBattle = battles.FirstOrDefault(battle => battle.AccumulatedWeight > roll, battles[0]);
        return rolledBattle;
    }

    public void Setup()
    {
        for (var i = 0; i < _totalWeightsByTier.Length; i++)
        {
            SetupWeightForTier(i);
        }
    }

    private static List<BattleStats> GenerateDefaultBattles()
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
        var result = new List<BattleStats> { tier0Bats2, tier0Crab, tier1Bats3, tier1BatCrab };
        return result;
    }
}