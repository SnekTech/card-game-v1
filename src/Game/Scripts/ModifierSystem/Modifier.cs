using System.Linq;

namespace CardGameV1.ModifierSystem;

public class Modifier
{
    public ModifierType Type { get; init; }

    private readonly Dictionary<string, ModifierValue> _modifierValues = [];

    public ModifierValue GetValue(string source)
    {
        return _modifierValues[source];
    }

    public void AddNewValue(ModifierValue value)
    {
        _modifierValues.TryAdd(value.Source, value);
        _modifierValues[value.Source].FlatValue = value.FlatValue;
        _modifierValues[value.Source].PercentValue = value.PercentValue;
    }

    public void RemoveValue(string source)
    {
        _modifierValues.Remove(source);
    }

    public void ClearValues() => _modifierValues.Clear();

    public int GetModifiedValue(int baseValue)
    {
        var flatResult = baseValue;
        var percentResult = 1f;

        flatResult += _modifierValues.Values.Where(v => v.Type == ModifierValueType.Flat).Sum(value => value.FlatValue);
        percentResult += _modifierValues.Values.Where(v => v.Type == ModifierValueType.PercentBased)
            .Sum(value => value.PercentValue);

        return Mathf.FloorToInt(flatResult * percentResult);
    }
}

public enum ModifierType
{
    DamageDealt,
    DamageTaken,
    CardCost,
    ShopCost,
    NoModifier,
}