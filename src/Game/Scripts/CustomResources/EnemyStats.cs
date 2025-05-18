using CardGameV1.EnemyAI;
using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class EnemyStats : Stats
{
    [Export]
    private PackedScene ai = null!;

    // todo: hook up enemy AI
    public EnemyActionPicker ActionPicker { get; private set; } = new();
}