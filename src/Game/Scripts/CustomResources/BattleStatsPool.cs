using System.Collections.Generic;
using System.Linq;
using CardGameV1.Character;
using Godot;

namespace CardGameV1.CustomResources;

public class BattleStatsPool
{
    public static readonly BattleStatsPool DefaultPool = new();

    private readonly List<BattleStats> _pool = GenerateDefaultBattles();
    private readonly float[] _totalWeightsByTier = [0, 0, 0];

    private List<BattleStats> GetAllBattlesForTier(int tier) => _pool.Where(battle => battle.Tier == tier).ToList();

    private void SetupWeightForTier(int tier)
    {
        var battles = GetAllBattlesForTier(tier);
        _totalWeightsByTier[tier] = 0;

        foreach (var battle in battles)
        {
            _totalWeightsByTier[tier] += battle.Weight;
            battle.AccumulatedWeight = _totalWeightsByTier[tier];
        }
    }

    public BattleStats? GetRandomBattleForTier(int tier)
    {
        var roll = GD.RandRange(0, _totalWeightsByTier[tier]);
        var battles = GetAllBattlesForTier(tier);

        var rolledBattle = battles.FirstOrDefault(battle => battle.AccumulatedWeight > roll);
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
            Enemies = [EnemyPool.Bat, EnemyPool.Bat],
        };
        var tier0Crab = new BattleStats
        {
            Tier = 0,
            Weight = 3f,
            GoldReward = (49, 68),
            Enemies = [EnemyPool.Crab],
        };
        var tier1Bats3 = new BattleStats
        {
            Tier = 1,
            Weight = 1f,
            GoldReward = (63, 81),
            Enemies = [EnemyPool.Bat, EnemyPool.Bat, EnemyPool.Bat],
        };
        var tier1BatCrab = new BattleStats
        {
            Tier = 1,
            Weight = 2f,
            GoldReward = (63, 81),
            Enemies = [EnemyPool.Bat, EnemyPool.Crab],
        };
        var richBoss = new BattleStats
        {
            Tier = 2,
            Weight = 2f,
            GoldReward = (630, 810),
            Enemies = [EnemyPool.Crab],
        };
        var result = new List<BattleStats> { tier0Bats2, tier0Crab, tier1Bats3, tier1BatCrab, richBoss };
        return result;
    }
}