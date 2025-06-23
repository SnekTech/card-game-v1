using System;
using System.Collections.Generic;
using System.Linq;
using CardGameV1.CustomResources.Cards;

namespace CardGameV1.CustomResources;

public class CardPile(List<Card> cards)
{
    public event Action<int>? CardPileSizeChanged;

    public CardPile() : this([])
    {
    }

    private readonly List<Card> _cards = cards.ToList();

    public bool IsEmpty => _cards.Count == 0;
    public List<Card> Cards => _cards.ToList();

    public Card DrawCard()
    {
        var card = _cards[0];
        _cards.RemoveAt(0);
        CardPileSizeChanged?.Invoke(_cards.Count);
        return card;
    }

    public void AddCard(Card card)
    {
        _cards.Add(card);
        CardPileSizeChanged?.Invoke(_cards.Count);
    }

    public void Shuffle() => _cards.Shuffle();

    public void Clear()
    {
        _cards.Clear();
        CardPileSizeChanged?.Invoke(_cards.Count);
    }

    public override string ToString()
    {
        var cardStrings = _cards.Select((t, i) => $"{i}: {t.Id}");

        return string.Join('\n', cardStrings);
    }
}