﻿namespace CardGameV1.EventBus;

public class PlayerEvents
{
    public event Action? PlayerHandDrawn;
    public event Action? PlayerHandDiscarded;
    public event Action? PlayerTurnEnded;
    public event Action? PlayerHit;
    public event Action? PlayerDied;

    public void EmitPlayerHandDrawn() => PlayerHandDrawn?.Invoke();
    public void EmitPlayerHandDiscarded() => PlayerHandDiscarded?.Invoke();
    public void EmitPlayerTurnEnded() => PlayerTurnEnded?.Invoke();
    public void EmitPlayerHit() => PlayerHit?.Invoke();
    public void EmitPlayerDied() => PlayerDied?.Invoke();
}