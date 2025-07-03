namespace CardGameV1.ModifierSystem;

public record ModifierValue(string Key, ModifierValueType Type)
{
    public float PercentValue { get; set; }
    public int FlatValue { get; set; }
}

public enum ModifierValueType
{
    PercentBased,
    Flat,
}