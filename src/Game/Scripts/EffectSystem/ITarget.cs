using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.EffectSystem;

public interface ITarget
{
    Vector2 GlobalPosition { get; }
    Stats Stats { get; }
    void TakeDamage(int amount);
}