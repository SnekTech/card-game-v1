namespace CardGameV1.WeightedRandom;

public record WeightData
{
    public required float Weight { get; init; }
    public float AccumulatedWeight { get; set; }
}