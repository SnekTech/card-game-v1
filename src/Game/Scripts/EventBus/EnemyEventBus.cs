using System;
using CardGameV1.Character;

namespace CardGameV1.EventBus;

public class EnemyEventBus
{
    public event Action<Enemy>? EnemyActionCompleted;
    public event Action? EnemyTurnEnded;

    public void EmitEnemyActionCompleted(Enemy enemy) => EnemyActionCompleted?.Invoke(enemy);
    public void EmitEnemyTurnEnded() => EnemyTurnEnded?.Invoke();
}