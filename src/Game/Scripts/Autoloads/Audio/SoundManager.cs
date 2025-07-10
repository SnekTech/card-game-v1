namespace CardGameV1.Autoloads.Audio;

[SceneTree]
public partial class SoundManager : Node
{
    public SoundPlayer MusicPlayer => _.musicPlayer;
    public SoundPlayer SFXPlayer => _.sfxPlayer;
}