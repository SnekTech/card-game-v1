namespace CardGameV1.ModifierSystem;

public record ModifierValue(string Source, ModifierValueType Type)
{
    public float PercentValue { get; set; }
    public int FlatValue { get; set; }
}

public enum ModifierValueType
{
    PercentBased,
    Flat,
}