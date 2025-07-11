using CardGameV1.Character;

namespace CardGameV1.EnemyAI;

public static class EnemyActionPickerFactory
{
    public static EnemyActionPicker CreateCrabBrain(Enemy enemy) => new(
        [EnemyConditionalAction.CreateCrabMegaBlockAction(enemy)],
        [EnemyChanceBasedAction.CreateCrabAttackAction(enemy), EnemyChanceBasedAction.CreateCrabBlockAction(enemy)]
    );

    public static EnemyActionPicker CreateBatBrain(Enemy enemy) =>
        new(
            [],
            [EnemyChanceBasedAction.CreateBatAttackAction(enemy), EnemyChanceBasedAction.CreateBatBlockAction(enemy)]
        );
}