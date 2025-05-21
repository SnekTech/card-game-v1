namespace CardGameV1.Autoload;

public partial class MusicPlayer : SoundPlayer
{
    public static MusicPlayer Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }
}