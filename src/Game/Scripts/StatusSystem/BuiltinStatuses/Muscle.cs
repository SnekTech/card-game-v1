using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class Muscle : Status
{
    public override void Init(ITarget target)
    {
        base.Init(target);
        Changed += OnStatusChanged;
        OnStatusChanged();
    }
    
    private void OnStatusChanged()
    {
        GD.Print($"Muscle status: +{Stacks} damage");
    }
}