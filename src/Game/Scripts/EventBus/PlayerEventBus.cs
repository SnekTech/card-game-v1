using System;

namespace CardGameV1.EventBus;

public class PlayerEventBus
{
    public event Action? PlayerHandDrawn;

    public void EmitPlayerHandDrawn() => PlayerHandDrawn?.Invoke();
}