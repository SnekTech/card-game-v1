using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.StatusSystem;
using Godot;

namespace CardGameV1.EffectSystem;

public interface ITarget
{
    Vector2 GlobalPosition { get; }
    Stats Stats { get; }
    StatusHandler StatusHandler { get; }
    Task TakeDamageAsync(int amount);
}