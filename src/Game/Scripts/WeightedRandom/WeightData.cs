namespace CardGameV1.WeightedRandom;

public record WeightData(float Weight)
{
    public float Weight { get; set; } = Weight;
    public float AccumulatedWeight { get; set; }
}