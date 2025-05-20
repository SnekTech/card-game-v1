using System.Threading.Tasks;
using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI;

public abstract partial class EnemyAction : Node
{
    [Export]
    public Intent Intent { get; private set; } = null!;

    public Enemy? Enemy { get; set; }
    public ITarget? Target { get; set; }

    public abstract Task PerformActionAsync();
}