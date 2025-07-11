using CardGameV1.Character;

namespace CardGameV1.EnemyAI;

public class EnemyChanceBasedAction : EnemyAction
{
    public required float ChanceWeight { get; init; }
    public float AccumulatedWeight { get; set; }

    public static EnemyChanceBasedAction CreateBatAttackAction(Enemy enemy)
    {
        const int batDamage = 4;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent($"{batDamage}", "res://art/tile_0103.png"),
            ActionPerformer = new Attack(batDamage) { Enemy = enemy },
            ChanceWeight = 3,
        };
    }

    public static EnemyChanceBasedAction CreateBatBlockAction(Enemy enemy)
    {
        const int batBlock = 4;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent("", "res://art/tile_0101.png"),
            ActionPerformer = new Block(batBlock) { Enemy = enemy },
            ChanceWeight = 1,
        };
    }

    public static EnemyChanceBasedAction CreateCrabAttackAction(Enemy enemy)
    {
        const int crabDamage = 7;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent($"{crabDamage}", "res://art/tile_0103.png"),
            ActionPerformer = new Attack(crabDamage) { Enemy = enemy },
            ChanceWeight = 1,
        };
    }

    public static EnemyChanceBasedAction CreateCrabBlockAction(Enemy enemy)
    {
        const int crabBlock = 6;
        return new EnemyChanceBasedAction
        {
            Intent = new Intent("", "res://art/tile_0101.png"),
            ActionPerformer = new Block(crabBlock) { Enemy = enemy },
            ChanceWeight = 1,
        };
    }
}