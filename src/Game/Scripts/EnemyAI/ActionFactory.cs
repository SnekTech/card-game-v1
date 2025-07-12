using CardGameV1.Character;
using CardGameV1.EnemyAI.ActionPerformers;
using CardGameV1.EnemyAI.Crab;
using CardGameV1.WeightedRandom;

namespace CardGameV1.EnemyAI;

public static class ActionFactory
{
    public static EnemyChanceBasedAction CreateBatAttackAction(Enemy enemy)
    {
        const int batDamage = 4;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent($"{batDamage}", "res://art/tile_0103.png"),
            ActionPerformer = new Attack(batDamage) { Enemy = enemy },
            WeightData = new WeightData(3),
        };
    }

    public static EnemyChanceBasedAction CreateBatBlockAction(Enemy enemy)
    {
        const int batBlock = 4;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent("", "res://art/tile_0101.png"),
            ActionPerformer = new Block(batBlock) { Enemy = enemy },
            WeightData = new WeightData(1),
        };
    }

    public static EnemyChanceBasedAction CreateCrabAttackAction(Enemy enemy)
    {
        const int crabDamage = 7;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent($"{crabDamage}", "res://art/tile_0103.png"),
            ActionPerformer = new Attack(crabDamage) { Enemy = enemy },
            WeightData = new WeightData(1),
        };
    }

    public static EnemyChanceBasedAction CreateCrabBlockAction(Enemy enemy)
    {
        const int crabBlock = 6;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent("", "res://art/tile_0101.png"),
            ActionPerformer = new Block(crabBlock) { Enemy = enemy },
            WeightData = new WeightData(1),
        };
    }

    public static EnemyConditionalAction CreateCrabMegaBlockAction(Enemy enemy)
    {
        const int megaBlockAmount = 15;
        const string megaBlockIconPath = "res://art/tile_0102.png";
        return new EnemyConditionalAction
        {
            Intent = new Intent("", megaBlockIconPath),
            ActionPerformer = new Block(megaBlockAmount) { Enemy = enemy },
            PerformDictator = new PerformMegaBlockDictator { Enemy = enemy },
        };
    }
}