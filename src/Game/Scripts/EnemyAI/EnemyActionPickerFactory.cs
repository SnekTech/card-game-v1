using CardGameV1.EnemyAI.Bat;
using CardGameV1.EnemyAI.Crab;

namespace CardGameV1.EnemyAI;

public static class EnemyActionPickerFactory
{
    public static readonly EnemyActionPicker Crab = new([new CrabMegaBlockAction()], [
        new CrabAttackAction(), new CrabBlockAction(),
    ]);

    public static readonly EnemyActionPicker Bat = new([], [
        new BatAttackAction(), new BatBlockAction(),
    ]);
}