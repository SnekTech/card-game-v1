using System.Linq;

namespace CardGameV1.ModifierSystem;

public class ModifierHandler
{
    private readonly List<Modifier> _modifiers = [];

    public void AddModifier(Modifier modifier) => _modifiers.Add(modifier);

    public bool HasModifier(ModifierType type) => _modifiers.Any(modifier => modifier.Type == type);
    
    public Modifier? GetModifier(ModifierType type) => _modifiers.FirstOrDefault(modifier => modifier.Type == type);

    public int GetModifiedValue(int baseValue, ModifierType type)
    {
        var modifier = GetModifier(type);
        return modifier?.GetModifiedValue(baseValue) ?? baseValue;
    }
}