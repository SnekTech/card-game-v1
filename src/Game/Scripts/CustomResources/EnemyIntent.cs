using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class EnemyIntent : Resource
{
    [Export]
    public string Number { get; set; } = string.Empty;

    [Export]
    public Texture2D Icon { get; private set; } = null!;
}