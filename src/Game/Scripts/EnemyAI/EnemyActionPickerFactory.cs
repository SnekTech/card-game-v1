using CardGameV1.Character;

namespace CardGameV1.EnemyAI;

public static class EnemyActionPickerFactory
{
    public static EnemyActionPicker CreateCrabBrain(Enemy enemy) => new(
        [ActionFactory.CreateCrabMegaBlockAction(enemy)],
        [ActionFactory.CreateCrabAttackAction(enemy), ActionFactory.CreateCrabBlockAction(enemy)]
    );

    public static EnemyActionPicker CreateBatBrain(Enemy enemy) =>
        new(
            [],
            [ActionFactory.CreateBatAttackAction(enemy), ActionFactory.CreateBatBlockAction(enemy)]
        );
}