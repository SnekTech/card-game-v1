using System;
using CardGameV1.CardVisual;
using CardGameV1.CustomResources;

namespace CardGameV1.EventBus;

public class CardEventBus
{
    public event Action<CardUI>? CardDragStarted;
    public event Action<CardUI>? CardDragEnded;
    public event Action<CardUI>? CardAimStarted;
    public event Action<CardUI>? CardAimEnded;
    public event Action<Card>? CardPlayed;

    public void EmitCardDragStared(CardUI cardUI) => CardDragStarted?.Invoke(cardUI);
    public void EmitCardDragEnded(CardUI cardUI) => CardDragEnded?.Invoke(cardUI);
    public void EmitCardAimStared(CardUI cardUI) => CardAimStarted?.Invoke(cardUI);
    public void EmitCardAimEnded(CardUI cardUI) => CardAimEnded?.Invoke(cardUI);
    public void EmitCardPlayed(Card card) => CardPlayed?.Invoke(card);
}