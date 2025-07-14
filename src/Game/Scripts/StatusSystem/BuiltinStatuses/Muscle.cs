using CardGameV1.EffectSystem;
using CardGameV1.ModifierSystem;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class Muscle : Status
{
    private ITarget _target = null!;

    public override string Tooltip => GenerateTooltip(Stacks);

    private static string GenerateTooltip(int muscleStacks) => $"{nameof(Muscle)}: attacks deal {muscleStacks} more damage";
    
    public override void Init(ITarget target)
    {
        _target = target;
        Changed += OnStatusChanged;
        OnStatusChanged();
    }

    public override Task ApplyStatusAsync(ITarget target, CancellationToken cancellationToken) => Task.CompletedTask;

    private void OnStatusChanged()
    {
        const string modiferKey = nameof(Muscle);
        var damageDealtModifier = _target.ModifierHandler.GetModifier(ModifierType.DamageDealt);
        if (damageDealtModifier == null)
        {
            GD.PrintErr($"no damage dealt modifier on this target for {modiferKey} status to work");
            return;
        }

        var muscleModifierValue = damageDealtModifier.GetValue(modiferKey) ??
                                  new ModifierValue(modiferKey, ModifierValueType.Flat);
        muscleModifierValue.FlatValue = Stacks;
        damageDealtModifier.AddNewValue(muscleModifierValue);

        GD.Print($"damage dealt with {modiferKey}: {_target.ModifierHandler.GetModifiedValue(0, ModifierType.DamageDealt)}");
    }
}