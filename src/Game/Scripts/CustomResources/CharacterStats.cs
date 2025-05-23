using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class CharacterStats : Stats
{
    [Export] public CardPile StartingDeck { get; private set; } = null!;
    [Export] public int CardsPerTurn { get; private set; } = 5;
    [Export] public int MaxMana { get; private set; } = 3;

    private int _mana;
    public CardPile Deck { get; set; } = new();
    public CardPile DrawPile { get; set; } = new();
    public CardPile DiscardPile { get; set; } = new();
    
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

    public override void TakeDamage(int damage)
    {
        var initialHealth = Health;
        base.TakeDamage(damage);
        if (Health < initialHealth)
        {
            EventBus.EventBusOwner.PlayerEvents.EmitPlayerHit();
        }
    }

    public bool CanPlayCard(Card card) => Mana >= card.Cost;

    public override CharacterStats CreateInstance()
    {
        var instance = (CharacterStats)base.CreateInstance();
        instance.ResetMana();
        instance.Deck = StartingDeck;
        instance.DrawPile = new CardPile();
        instance.DiscardPile = new CardPile();
        return instance;
    }
}