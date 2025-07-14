using CardGameV1.Character;
using CardGameV1.WeightedRandom;

namespace CardGameV1.CustomResources;

public class BattleStatsPool
{
    public static readonly BattleStatsPool DefaultPool = new();

    private readonly List<BattleStats> _pool = GenerateDefaultBattles();

    private List<BattleStats> GetAllBattlesForTier(int tier) => _pool.Where(battle => battle.Tier == tier).ToList();

    public BattleStats GetRandomBattleForTier(int tier) => GetAllBattlesForTier(tier).GetWeighted();

    private static List<BattleStats> GenerateDefaultBattles()
    {
        var tier0Bats2 = new BattleStats
        {
            Tier = 0,
            GoldReward = (53, 72),
            Enemies = [EnemyPool.Bat, EnemyPool.Bat],
            Weight = 2.5f,
        };
        var tier0Crab = new BattleStats
        {
            Tier = 0,
            GoldReward = (49, 68),
            Enemies = [EnemyPool.Crab],
            Weight = 3f,
        };
        var tier1Bats3 = new BattleStats
        {
            Tier = 1,
            GoldReward = (63, 81),
            Enemies = [EnemyPool.Bat, EnemyPool.Bat, EnemyPool.Bat],
            Weight = 1f,
        };
        var tier1BatCrab = new BattleStats
        {
            Tier = 1,
            GoldReward = (63, 81),
            Enemies = [EnemyPool.Bat, EnemyPool.Crab],
            Weight = 2f,
        };
        var richBoss = new BattleStats
        {
            Tier = 2,
            GoldReward = (630, 810),
            Enemies = [EnemyPool.Crab],
            Weight = 2f,
        };
        var result = new List<BattleStats>
        {
            tier0Bats2,
            tier0Crab,
            tier1Bats3,
            tier1BatCrab,
            richBoss,
        };
        return result;
    }
}