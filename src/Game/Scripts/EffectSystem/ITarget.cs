using System.Threading.Tasks;
using CardGameV1.CustomResources;
using Godot;

namespace CardGameV1.EffectSystem;

public interface ITarget
{
    Vector2 GlobalPosition { get; }
    Stats Stats { get; }
    Task TakeDamageAsync(int amount);
}