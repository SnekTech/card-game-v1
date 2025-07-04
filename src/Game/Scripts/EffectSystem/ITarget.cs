using System.Threading;
using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;

namespace CardGameV1.EffectSystem;

public interface ITarget
{
    Vector2 GlobalPosition { get; }
    Stats Stats { get; }
    StatusHandler StatusHandler { get; }
    ModifierHandler ModifierHandler { get; }
    Task TakeDamageAsync(int amount, ModifierType whichModifier);
    CancellationToken CancellationTokenOnQueueFree { get; }
    void QFree();
}