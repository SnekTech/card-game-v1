namespace CardGameV1.WeightedRandom;

public static class WeightedRandomCalculator
{
    public static T GetWeighted<T>(List<T> candidates) where T : class, IWeightedCandidate
    {
        if (candidates.Count == 0)
        {
            throw new ArgumentException("cannot get weighted random from an empty candidate list");
        }

        float totalWeight;
        SetupAccumulatedWeights();

        var roll = GD.RandRange(0f, totalWeight);
        foreach (var candidate in candidates)
        {
            if (candidate.WeightData.AccumulatedWeight > roll)
            {
                return candidate;
            }
        }

        return candidates[0];

        void SetupAccumulatedWeights()
        {
            totalWeight = 0;
            foreach (var candidate in candidates)
            {
                totalWeight += candidate.WeightData.Weight;
                candidate.WeightData.AccumulatedWeight = totalWeight;
            }
        }
    }
}