using CardGameV1.CustomResources.Cards;
using CardGameV1.WeightedRandom;

namespace CardGameV1.CustomResources.Run;

public class CardRarityWeightStats
{
    private const float BaseCommonWeight = 6f;
    private const float BaseUncommonWeight = 3.7f;
    private const float BaseRareWeight = 0.3f;

    private readonly Dictionary<CardRarity, WeightedCardRarity> _weightStats = new()
    {
        [CardRarity.Common] = new WeightedCardRarity
            { Rarity = CardRarity.Common, Weight = BaseCommonWeight },
        [CardRarity.Uncommon] = new WeightedCardRarity
            { Rarity = CardRarity.Uncommon, Weight = BaseUncommonWeight },
        [CardRarity.Rare] = new WeightedCardRarity
            { Rarity = CardRarity.Rare, Weight = BaseRareWeight },
    };

    public CardRarity GetWeightedRarity() =>
        WeightedRandomCalculator.GetWeighted(_weightStats.Values.ToList()).Rarity;

    public void ModifyWeights(CardRarity rarityRolled)
    {
        if (rarityRolled == CardRarity.Rare)
        {
            var rareWeight = _weightStats[CardRarity.Rare];
            rareWeight.Weight = Mathf.Clamp(rareWeight.Weight + 0.3f, BaseRareWeight, 5f);
        }
    }

    public record WeightedCardRarity : IWeightedCandidate
    {
        public required CardRarity Rarity { get; init; }
        public required float Weight { get; set; }
    }
}