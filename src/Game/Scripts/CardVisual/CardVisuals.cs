using CardGameV1.CustomResources.Cards;
using CardGameV1.CustomResources.Cards.Warrior;

namespace CardGameV1.CardVisual;

[SceneTree]
public partial class CardVisuals : Control
{
    private static readonly Dictionary<CardRarity, Color> RarityColors = new()
    {
        [CardRarity.Common] = Colors.Gray,
        [CardRarity.Uncommon] = Colors.CornflowerBlue,
        [CardRarity.Rare] = Colors.Gold,
    };

    private readonly Card defaultCard = CardPool.Get<WarriorAxeAttack>();
    private Card? _card;

    public Card Card
    {
        set
        {
            _card = value;
            UpdateContent(_card);
        }
    }

    public override void _Ready()
    {
        Card = defaultCard;
    }

    private void UpdateContent(Card card)
    {
        Cost.Text = card.Cost.ToString();
        Icon.Texture = card.Icon;
        Rarity.Modulate = RarityColors[card.Rarity];
    }
}