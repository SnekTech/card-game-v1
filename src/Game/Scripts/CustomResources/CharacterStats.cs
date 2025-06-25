using CardGameV1.CustomResources.Cards;
using Godot;

namespace CardGameV1.CustomResources;

public class CharacterStats(CardPile startingDeck) : Stats
{
    public string CharacterName { get; init; } = "default name";
    public string Description { get; init; } = "default description";
    public required string PortraitPath { get; init; }
    public required CardPile DraftableCards { get; init; }

    public Texture2D Portrait => SnekUtility.LoadTexture(PortraitPath);

    public int CardsPerTurn { get; private set; } = 5;

    public int MaxMana => 3;

    private int _mana;
    public CardPile Deck { get; private set; } = new(startingDeck.Cards);
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

    public CharacterStats CreateInstance()
    {
        var other = new CharacterStats(startingDeck)
        {
            MaxHealth = MaxHealth,
            Health = MaxHealth,
            Block = Block,
            ArtPath = ArtPath,
            CharacterName = CharacterName,
            Description = Description,
            PortraitPath = PortraitPath,
            DraftableCards = DraftableCards,
            CardsPerTurn = CardsPerTurn,
        };
        other.ResetMana();

        return other;
    }
}