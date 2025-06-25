using CardGameV1.EnemyAI.Bat;
using CardGameV1.EnemyAI.Crab;

namespace CardGameV1.EnemyAI;

public static class EnemyActionPickerFactory
{
    public static EnemyActionPicker CreateCrabBrain() => new(
        [new CrabMegaBlockAction()],
        [new CrabAttackAction(), new CrabBlockAction()]
    );

    public static EnemyActionPicker CreateBatBrain() =>
        new(
            [],
            [new BatAttackAction(), new BatBlockAction()]
        );
}