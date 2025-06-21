using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class TrueStrengthForm : Status
{
    private const int StacksPerTurn = 2;
    
    public override async Task ApplyStatusAsync(ITarget target)
    {
        GD.Print($"applying {nameof(TrueStrengthForm)}");

        var muscle = StatusFactory.Muscle;
        muscle.Stacks = StacksPerTurn;
        var addMuscleEffect = new AddStatusEffect(muscle);
        await addMuscleEffect.ExecuteAllAsync([target]);
    }
}