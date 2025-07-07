using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class Exposed : Status
{
    private const string ModifierKey = nameof(Exposed);

    private const float Increment = 0.5f;
    private Modifier? _damageTakenModifier;
    private bool _subscribed;

    public override string Tooltip => GenerateTooltip(Duration);

    public override void Init(ITarget target)
    {
        _damageTakenModifier = target.ModifierHandler.GetModifier(ModifierType.DamageTaken);
        if (_damageTakenModifier == null)
        {
            GD.PrintErr($"no damage taken modifier on target {target}");
            return;
        }

        var exposedModifierValue = _damageTakenModifier.GetValue(ModifierKey) ??
                                   new ModifierValue(ModifierKey, ModifierValueType.PercentBased);
        exposedModifierValue.PercentValue = Increment;
        _damageTakenModifier.AddNewValue(exposedModifierValue);
        if (!_subscribed)
        {
            Changed += OnStatusChanged;
            _subscribed = true;
        }
    }

    private static string GenerateTooltip(int duration, float increment = Increment) =>
        $"{nameof(Exposed)}: takes {increment:P0} more damage for {duration} turns";

    private void OnStatusChanged()
    {
        if (Duration <= 0 && _damageTakenModifier != null)
        {
            _damageTakenModifier.RemoveValue(ModifierKey);
        }
    }
}