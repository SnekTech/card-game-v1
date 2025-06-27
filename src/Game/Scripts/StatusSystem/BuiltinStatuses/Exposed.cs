using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.StatusSystem.BuiltinStatuses;

public class Exposed : Status
{
    private const float Modifier = 0.5f;

    public override async Task ApplyStatusAsync(ITarget target, CancellationToken cancellationToken)
    {
        GD.Print($"{target} should take {Modifier:P0} more damage");

        // todo: replace with modifier system
        var damageEffect = new DamageEffect(12);
        await damageEffect.ExecuteAllAsync([target], cancellationToken);
    }
}