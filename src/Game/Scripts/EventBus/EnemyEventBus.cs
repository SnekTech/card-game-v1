using System;

namespace CardGameV1.EventBus;

public class EnemyEventBus
{
    public event Action? EnemyTurnEnded;

    public void EmitEnemyTurnEnded() => EnemyTurnEnded?.Invoke();
}