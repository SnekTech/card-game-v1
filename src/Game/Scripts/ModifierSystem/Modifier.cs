using System.Linq;

namespace CardGameV1.ModifierSystem;

public class Modifier
{
    public ModifierType Type { get; init; }

    private readonly Dictionary<string, ModifierValue> _modifierValues = [];

    public ModifierValue? GetValue(string key)
    {
        return _modifierValues.GetValueOrDefault(key);
    }

    public void AddNewValue(ModifierValue value)
    {
        _modifierValues.TryAdd(value.Key, value);
        _modifierValues[value.Key].FlatValue = value.FlatValue;
        _modifierValues[value.Key].PercentValue = value.PercentValue;
    }

    public void RemoveValue(string key)
    {
        _modifierValues.Remove(key);
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