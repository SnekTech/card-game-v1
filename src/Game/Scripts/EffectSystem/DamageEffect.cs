using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGameV1.Autoload;

namespace CardGameV1.EffectSystem;

public class DamageEffect(int amount) : Effect
{
    public override async Task ExecuteAllAsync(IEnumerable<ITarget> targets)
    {
        var tasks = targets.Select(PlaySoundAndTakeDamage);
        await Task.WhenAll(tasks);
        return;

        async Task PlaySoundAndTakeDamage(ITarget target)
        {
            if (Sound != null)
            {
                SoundManager.SFXPlayer.Play(Sound);
            }

            await target.TakeDamageAsync(amount);
        }
    }
}