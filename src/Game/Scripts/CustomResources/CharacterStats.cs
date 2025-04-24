using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class CharacterStats : Stats
{
    [Export] public CardPile StartingDeck { get; private set; } = null!;
    [Export] public int CardsPerTurn { get; private set; } = 5;
    [Export] public int MaxMana { get; private set; } = 3;

    private int _mana;
    private CardPile _deck = new();
    private CardPile _drawPile = new();
    private CardPile _discardPile = new();
    
    public int Mana
    {
        get => _mana;
        set
        {
            _mana = value;
            EmitStatsChanged();
        }
    }

    public void ResetMana() => Mana = MaxMana;

    public bool CanPlayCard(Card card) => Mana >= card.Cost;

    public override CharacterStats CreateInstance()
    {
        var instance = (CharacterStats)base.CreateInstance();
        instance.ResetMana();
        instance._deck = StartingDeck;
        instance._drawPile = new CardPile();
        instance._discardPile = new CardPile();
        return instance;
    }
}