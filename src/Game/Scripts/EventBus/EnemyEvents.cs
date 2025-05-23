using System;

namespace CardGameV1.EventBus;

public class EnemyEvents
{
    public event Action? EnemyTurnEnded;

    public void EmitEnemyTurnEnded() => EnemyTurnEnded?.Invoke();
}