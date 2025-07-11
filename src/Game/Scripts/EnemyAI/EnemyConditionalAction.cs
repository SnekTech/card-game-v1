using CardGameV1.Character;

namespace CardGameV1.EnemyAI;

public class EnemyConditionalAction : EnemyAction
{
    public bool IsPerformable() => false;

    public static EnemyConditionalAction CreateCrabMegaBlockAction(Enemy enemy)
    {
        const int megaBlockAmount = 15;
        const string megaBlockIconPath = "res://art/tile_0102.png";
        return new EnemyConditionalAction
        {
            Intent = new Intent("", megaBlockIconPath),
            ActionPerformer = new Block(megaBlockAmount) { Enemy = enemy },
        };
    }
}