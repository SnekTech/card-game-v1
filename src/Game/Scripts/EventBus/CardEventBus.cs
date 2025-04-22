using System;
using CardGameV1.CardVisual;

namespace CardGameV1.EventBus;

public class CardEventBus
{
    public event Action<CardUI>? CardAimStarted;
    public event Action<CardUI>? CardAimEnded;

    public void EmitCardAimStared(CardUI cardUI) => CardAimStarted?.Invoke(cardUI);
    public void EmitCardAimEnded(CardUI cardUI) => CardAimEnded?.Invoke(cardUI);
}