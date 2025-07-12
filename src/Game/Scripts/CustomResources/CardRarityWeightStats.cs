using CardGameV1.CustomResources.Cards;
using CardGameV1.WeightedRandom;

namespace CardGameV1.CustomResources;

public class CardRarityWeightStats
{
    private const float BaseCommonWeight = 6f;
    private const float BaseUncommonWeight = 3.7f;
    private const float BaseRareWeight = 0.3f;

    private readonly Dictionary<CardRarity, WeightedCardRarity> _weightStats = new()
    {
        [CardRarity.Common] = new WeightedCardRarity
            { Rarity = CardRarity.Common, WeightData = new WeightData(BaseCommonWeight) },
        [CardRarity.Uncommon] = new WeightedCardRarity
            { Rarity = CardRarity.Uncommon, WeightData = new WeightData(BaseUncommonWeight) },
        [CardRarity.Rare] = new WeightedCardRarity
            { Rarity = CardRarity.Rare, WeightData = new WeightData(BaseRareWeight) },
    };

    public CardRarity GetWeightedRarity() =>
        WeightedRandomCalculator.GetWeighted(_weightStats.Values.ToList()).Rarity;

    public void ModifyWeights(CardRarity rarityRolled)
    {
        if (rarityRolled == CardRarity.Rare)
        {
            var rareWeight = _weightStats[CardRarity.Rare];
            rareWeight.WeightData.Weight =
                Mathf.Clamp(rareWeight.WeightData.Weight + 0.3f, BaseRareWeight, 5f);
        }
    }

    public record WeightedCardRarity : IWeightedCandidate
    {
        public required CardRarity Rarity { get; init; }
        public required WeightData WeightData { get; init; }
    }
}