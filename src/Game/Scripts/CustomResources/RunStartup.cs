using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class RunStartup : Resource
{
    public enum RunType
    {
        NewRun,
        ContinuedRun
    }

    public RunType Type { get; set; } = RunType.NewRun;

    public CharacterStats PickedCharacter { get; set; } = null!;
}