using CardGameV1.Constants;
using CardGameV1.CustomResources.Cards;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileDisplay;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[Scene]
public partial class CardRewards : ColorRect
{
    public event Action<Card?>? CardRewardSelected;

    [Node]
    private HBoxContainer cards = null!;
    [Node]
    private Button skipCardButton = null!;
    [Node]
    private Button takeButton = null!;
    [Node]
    private CardTooltipPopup cardTooltipPopup = null!;

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
                cards.AddChild(newCard);
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
        takeButton.Pressed += OnTakeButtonPressed;
        skipCardButton.Pressed += OnSkipButtonPressed;
    }

    public override void _ExitTree()
    {
        ClearRewards();
        takeButton.Pressed -= OnTakeButtonPressed;
        skipCardButton.Pressed -= OnSkipButtonPressed;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(BuiltinInputActions.UICancel))
        {
            cardTooltipPopup.HideTooltip();
        }
    }

    private void ClearRewards()
    {
        _rewards.Clear();
        foreach (var child in cards.GetChildrenOfType<CardMenuUI>())
        {
            child.TooltipRequested -= OnCardTooltipRequested;
        }
        cards.ClearChildren();
        cardTooltipPopup.HideTooltip();
        _selectedCard = null;
    }

    private void OnCardTooltipRequested(Card card)
    {
        _selectedCard = card;
        cardTooltipPopup.ShowTooltip(card);
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

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}