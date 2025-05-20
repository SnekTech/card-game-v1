using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class EnemyStats : Stats
{
    [Export]
    public PackedScene AIScene { get; private set; } = null!;
}