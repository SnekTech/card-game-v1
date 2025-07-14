using CardGameV1.CustomResources.Cards;
using CardGameV1.WeightedRandom;

namespace CardGameV1.CustomResources.Run;

public class CardRarityWeightStats
{
    private const float BaseCommonWeight = 6f;
    private const float BaseUncommonWeight = 3.7f;
    private const float BaseRareWeight = 0.3f;

    private readonly WeightedCardRarity _common = new() { Rarity = CardRarity.Common, Weight = BaseCommonWeight };
    private readonly WeightedCardRarity _uncommon = new() { Rarity = CardRarity.Uncommon, Weight = BaseUncommonWeight };
    private readonly WeightedCardRarity _rare = new() { Rarity = CardRarity.Rare, Weight = BaseRareWeight };

    public CardRarity GetWeightedRarity()
    {
        List<WeightedCardRarity> list = [_common, _uncommon, _rare];
        return list.GetWeighted().Rarity;
    }

    public void ModifyWeights(CardRarity rarityRolled)
    {
        if (rarityRolled == CardRarity.Rare)
        {
            ResetRareWeight();
        }
        else
        {
            RaiseSomeRareWeight();
        }

        return;

        void RaiseSomeRareWeight() => _rare.Weight = Mathf.Clamp(_rare.Weight + 0.3f, BaseRareWeight, 5f);
        void ResetRareWeight() => _rare.Weight = BaseRareWeight;
    }

    public record WeightedCardRarity : IWeightedCandidate
    {
        public required CardRarity Rarity { get; init; }
        public required float Weight { get; set; }
    }
}