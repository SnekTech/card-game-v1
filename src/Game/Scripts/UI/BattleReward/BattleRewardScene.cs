using System.Collections.Generic;
using System.Linq;
using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;
using CardGameV1.EventBus;
using CardGameV1.MyExtensions;
using Godot;
using Godot.Collections;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[Scene]
public partial class BattleRewardScene : Control
{
    [Node]
    private VBoxContainer rewards = null!;
    [Node]
    private Button backButton = null!;

    private static readonly Texture2D GoldIcon = GD.Load<Texture2D>("res://art/gold.png");
    private static readonly Texture2D CardIcon = GD.Load<Texture2D>("res://art/rarity.png");
    private readonly System.Collections.Generic.Dictionary<CardRarity, float> _cardRarityWeights = new()
    {
        [CardRarity.Common] = 0,
        [CardRarity.Uncommon] = 0,
        [CardRarity.Rare] = 0
    };

    private float _cardRewardTotalWeight;

    public RunStats RunStats { private get; set; } = new();
    public CharacterStats CharacterStats { private get; set; } = null!;

    public override void _Ready()
    {
        rewards.ClearChildren();
    }

    public override void _EnterTree()
    {
        backButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        backButton.Pressed -= OnBackButtonPressed;
    }

    public void AddGoldReward(int amount)
    {
        var goldRewardButton = SceneFactory.Instantiate<RewardButton>();
        goldRewardButton.RewardIcon = GoldIcon;
        goldRewardButton.RewardText = $"{amount} gold";
        goldRewardButton.Pressed += () => RunStats.Gold += amount;
        Callable.From(() => rewards.AddChild(goldRewardButton)).CallDeferred();
    }

    public void AddCardReward()
    {
        var cardRewardButton = SceneFactory.Instantiate<RewardButton>();
        cardRewardButton.RewardIcon = CardIcon;
        cardRewardButton.RewardText = "Add New Card";
        cardRewardButton.Pressed += ShowCardRewards;
        Callable.From(() => rewards.AddChild(cardRewardButton)).CallDeferred();
    }

    private void ShowCardRewards()
    {
        var cardRewards = SceneFactory.Instantiate<CardRewards>();
        AddChild(cardRewards);
        cardRewards.CardRewardSelected += OnCardRewardSelected;

        var cardRewardList = new List<Card>();
        var availableCards = CharacterStats.DraftableCards.Cards.Duplicate(true);

        for (var i = 0; i < RunStats.CardRewards; i++)
        {
            SetupCardChances();
            var roll = GD.RandRange(0, _cardRewardTotalWeight);

            foreach (var (rarity, weight) in _cardRarityWeights)
            {
                if (weight > roll)
                {
                    ModifyWeights(rarity);
                    var pickedCard = GetRandomAvailableCard(availableCards, rarity);
                    cardRewardList.Add(pickedCard);
                    availableCards.Remove(pickedCard);
                    break;
                }
            }
        }

        cardRewards.Rewards = cardRewardList;
        cardRewards.Show();
    }

    private void SetupCardChances()
    {
        _cardRewardTotalWeight = RunStats.CommonWeight + RunStats.UncommonWeight + RunStats.RareWeight;
        _cardRarityWeights[CardRarity.Common] = RunStats.CommonWeight;
        _cardRarityWeights[CardRarity.Uncommon] = RunStats.CommonWeight + RunStats.UncommonWeight;
        _cardRarityWeights[CardRarity.Rare] = _cardRewardTotalWeight;
    }

    private void ModifyWeights(CardRarity rarityRolled)
    {
        RunStats.RareWeight = rarityRolled == CardRarity.Rare
            ? RunStats.BaseRareWeight
            : Mathf.Clamp(RunStats.RareWeight + 0.3f, RunStats.BaseRareWeight, 5f);
    }

    private Card GetRandomAvailableCard(Array<Card> availableCards, CardRarity rarity)
    {
        var cardsWithThisRarity = availableCards.Where(card => card.Rarity == rarity).ToList();
        return cardsWithThisRarity.PickRandom();
    }

    private void OnCardRewardSelected(Card? card)
    {
        if (card == null)
            return;

        CharacterStats.Deck.AddCard(card);
    }

    private void OnBackButtonPressed() => EventBusOwner.Events.EmitBattleRewardExited();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}