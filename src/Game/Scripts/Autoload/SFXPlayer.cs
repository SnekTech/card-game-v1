namespace CardGameV1.Autoload;

public partial class SFXPlayer : SoundPlayer
{
    public static SFXPlayer Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }
}