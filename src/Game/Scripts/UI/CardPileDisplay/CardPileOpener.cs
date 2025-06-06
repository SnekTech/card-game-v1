using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.UI.CardPileDisplay;

public partial class CardPileOpener : TextureButton
{
    [Export]
    private Label counter = null!;

    private CardPile? _cardPile;

    public CardPile CardPile
    {
        set
        {
            if (_cardPile != null)
            {
                _cardPile.CardPileSizeChanged -= OnCardPileSizeChanged;
            }

            _cardPile = value;
            _cardPile.CardPileSizeChanged += OnCardPileSizeChanged;

            UpdateCounter(_cardPile.Cards.Count);
        }
    }

    public override void _ExitTree()
    {
        if (_cardPile != null)
        {
            _cardPile.CardPileSizeChanged -= OnCardPileSizeChanged;
        }
    }

    private void UpdateCounter(int count) => counter.Text = count.ToString();

    private void OnCardPileSizeChanged(int cardsAmount) => UpdateCounter(cardsAmount);
}