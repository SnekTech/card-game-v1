using CardGameV1.CustomResources;

namespace CardGameV1.EffectSystem;

public interface ITarget
{
    Stats Stats { get; }
    void TakeDamage(int amount);
}