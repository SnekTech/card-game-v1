using CardGameV1.Constants;
using CardGameV1.CustomResources.Cards;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileDisplay;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[SceneTree]
public partial class CardRewards : ColorRect
{
    public event Action<Card?>? CardRewardSelected;

    private readonly List<Card> _rewards = [];
    private Card? _selectedCard;

    public List<Card> Rewards
    {
        set
        {
            ClearRewards();

            _rewards.AddRange(value);
            _selectedCard = _rewards[0];
            
            foreach (var card in _rewards)
            {
                var newCard = SceneFactory.Instantiate<CardMenuUI>();
                Cards.AddChild(newCard);
                newCard.Card = card;
                newCard.TooltipRequested += OnCardTooltipRequested;
            }
        }
    }

    public override void _Ready()
    {
        ClearRewards();
    }

    public override void _EnterTree()
    {
        TakeButton.Pressed += OnTakeButtonPressed;
        SkipCardButton.Pressed += OnSkipButtonPressed;
    }

    public override void _ExitTree()
    {
        ClearRewards();
        TakeButton.Pressed -= OnTakeButtonPressed;
        SkipCardButton.Pressed -= OnSkipButtonPressed;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(BuiltinInputActions.UICancel))
        {
            CardTooltipPopup.HideTooltip();
        }
    }

    private void ClearRewards()
    {
        _rewards.Clear();
        foreach (var child in Cards.GetChildrenOfType<CardMenuUI>())
        {
            child.TooltipRequested -= OnCardTooltipRequested;
        }
        Cards.ClearChildren();
        CardTooltipPopup.HideTooltip();
        _selectedCard = null;
    }

    private void OnCardTooltipRequested(Card card)
    {
        _selectedCard = card;
        CardTooltipPopup.ShowTooltip(card);
    }

    private void OnTakeButtonPressed()
    {
        if (_selectedCard == null)
            return;

        CardRewardSelected?.Invoke(_selectedCard);
        QueueFree();
    }

    private void OnSkipButtonPressed()
    {
        CardRewardSelected?.Invoke(null);
        QueueFree();
    }
}