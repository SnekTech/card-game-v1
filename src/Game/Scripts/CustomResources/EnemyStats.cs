using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class EnemyStats : Stats
{
    [Export]
    private PackedScene ai = null!;
}