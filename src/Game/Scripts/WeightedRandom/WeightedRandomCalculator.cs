namespace CardGameV1.WeightedRandom;

public static class WeightedRandomCalculator
{
    public static T GetWeighted<T>(List<T> candidates) where T : class, IWeightedCandidate
    {
        if (candidates.Count == 0)
        {
            throw new ArgumentException("cannot get weighted random from an empty candidate list");
        }

        var accumulatedWeights = GetAccumulatedWeights();
        var roll = GD.RandRange(0f, accumulatedWeights[^1]);
        for (var i = 0; i < candidates.Count; i++)
        {
            if (accumulatedWeights[i] > roll)
            {
                return candidates[i];
            }
        }

        return candidates[0];

        float[] GetAccumulatedWeights()
        {
            var accumulated = new float[candidates.Count];
            accumulated[0] = candidates[0].Weight;
            for (var i = 1; i < candidates.Count; i++)
            {
                accumulated[i] = accumulated[i - 1] + candidates[i].Weight;
            }

            return accumulated;
        }
    }
}