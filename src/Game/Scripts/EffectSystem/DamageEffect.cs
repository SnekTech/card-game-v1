using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardGameV1.Autoload;
using CardGameV1.ModifierSystem;

namespace CardGameV1.EffectSystem;

public class DamageEffect(int amount, ModifierType receiverModifierType = ModifierType.DamageTaken) : Effect
{
    public override async Task ExecuteAllAsync(IEnumerable<ITarget> targets, CancellationToken cancellationToken)
    {
        var targetList = targets.ToList();
        foreach (var target in targetList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            target.CancellationTokenOnQueueFree.ThrowIfCancellationRequested();

            if (Sound != null)
            {
                SoundManager.SFXPlayer.Play(Sound);
            }

            await target.TakeDamageAsync(amount, receiverModifierType);
        }
    }
}