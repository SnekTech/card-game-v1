﻿using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;
using CardGameV1.CustomResources.Run;
using CardGameV1.EventBus;
using CardGameV1.MyExtensions;

namespace CardGameV1.UI.BattleReward;

[SceneTree]
public partial class BattleRewardScene : Control
{
    private static readonly Texture2D GoldIcon = SnekUtility.LoadTexture("res://art/gold.png");
    private static readonly Texture2D CardIcon = SnekUtility.LoadTexture("res://art/rarity.png");

    public RunStats RunStats { private get; set; } = new();
    public CharacterStats CharacterStats { private get; set; } = null!;

    public override void _Ready()
    {
        Rewards.ClearChildren();
    }

    public override void _EnterTree()
    {
        BackButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        BackButton.Pressed -= OnBackButtonPressed;
    }

    public void AddGoldReward(int amount)
    {
        var goldRewardButton = SceneFactory.Instantiate<RewardButton>();
        goldRewardButton.RewardIcon = GoldIcon;
        goldRewardButton.RewardText = $"{amount} gold";
        goldRewardButton.Pressed += () => RunStats.Gold += amount;
        Callable.From(() => Rewards.AddChild(goldRewardButton)).CallDeferred();
    }

    public void AddCardReward()
    {
        var cardRewardButton = SceneFactory.Instantiate<RewardButton>();
        cardRewardButton.RewardIcon = CardIcon;
        cardRewardButton.RewardText = "Add New Card";
        cardRewardButton.Pressed += ShowCardRewards;
        Callable.From(() => Rewards.AddChild(cardRewardButton)).CallDeferred();
    }

    private void ShowCardRewards()
    {
        var cardRewards = SceneFactory.Instantiate<CardRewards>();
        AddChild(cardRewards);
        cardRewards.CardRewardSelected += OnCardRewardSelected;

        var cardRewardList = new List<Card>();
        var availableCards = CharacterStats.DraftableCards.Cards;

        for (var i = 0; i < RunStats.CardRewards; i++)
        {
            var rarityRolled = RunStats.CardRarityWeightStats.GetWeightedRarity();
            RunStats.CardRarityWeightStats.ModifyWeights(rarityRolled);
            var pickedCard = availableCards.Where(card => card.Rarity == rarityRolled).ToList().PickRandom();
            cardRewardList.Add(pickedCard);
            availableCards.Remove(pickedCard);
        }

        cardRewards.Rewards = cardRewardList;
        cardRewards.Show();
    }

    private void OnCardRewardSelected(Card? card)
    {
        if (card == null)
            return;

        CharacterStats.Deck.AddCard(card);
    }

    private void OnBackButtonPressed() => EventBusOwner.Events.EmitBattleRewardExited();
}