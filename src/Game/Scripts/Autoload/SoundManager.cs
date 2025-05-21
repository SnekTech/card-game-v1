using Godot;
using GodotUtilities;

namespace CardGameV1.Autoload;

[Scene]
public partial class SoundManager : Node
{
    [Node]
    private SoundPlayer musicPlayer = null!;

    [Node]
    private SoundPlayer sfxPlayer = null!;

    public static SoundPlayer MusicPlayer { get; private set; } = null!;
    public static SoundPlayer SFXPlayer { get; private set; } = null!;

    public override void _Ready()
    {
        MusicPlayer = musicPlayer;
        SFXPlayer = sfxPlayer;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}