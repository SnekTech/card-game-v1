using System;
using System.Linq;
using CardGameV1.CustomResources.Cards;
using Godot;
using Godot.Collections;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class CardPile : Resource
{
    public event Action<int>? CardPileSizeChanged;

    [Export]
    private Array<Card> cards = [];

    public bool IsEmpty => cards.Count == 0;
    public Array<Card> Cards => cards.Duplicate();


    public Card DrawCard()
    {
        var card = cards[0];
        cards.RemoveAt(0);
        CardPileSizeChanged?.Invoke(cards.Count);
        return card;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
        CardPileSizeChanged?.Invoke(cards.Count);
    }

    public void Shuffle() => cards.Shuffle();

    public void Clear()
    {
        cards.Clear();
        CardPileSizeChanged?.Invoke(cards.Count);
    }

    public override string ToString()
    {
        var cardStrings = cards.Select((t, i) => $"{i}: {t.Id}");

        return string.Join('\n', cardStrings);
    }
}