using CardGameV1.CustomResources.Cards;
using CardGameV1.CustomResources.Cards.Warrior;
using Godot;
using GodotUtilities;

namespace CardGameV1.CardVisual;

[Scene]
public partial class CardVisuals : Control
{
    [Node]
    private Panel panel = null!;
    [Node]
    private Label cost = null!;
    [Node]
    private TextureRect icon = null!;
    [Node]
    private TextureRect rarity = null!;

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

    public Panel Panel => panel;
    public Label Cost => cost;
    public TextureRect Icon => icon;

    public override void _Ready()
    {
        Card = defaultCard;
    }

    private void UpdateContent(Card card)
    {
        cost.Text = card.Cost.ToString();
        icon.Texture = card.Icon;
        rarity.Modulate = card.RarityColor;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}