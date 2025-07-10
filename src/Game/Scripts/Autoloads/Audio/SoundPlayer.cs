using GodotUtilities;

namespace CardGameV1.Autoloads.Audio;

public partial class SoundPlayer : Node
{
    public void Play(AudioStream audioStream, bool single = false)
    {
        if (single)
            Stop();

        foreach (var player in this.GetChildrenOfType<AudioStreamPlayer>())
        {
            if (player.IsPlaying() == false)
            {
                player.Stream = audioStream;
                player.Play();
                break;
            }
        }
    }

    private void Stop()
    {
        foreach (var player in this.GetChildrenOfType<AudioStreamPlayer>())
        {
            player.Stop();
        }
    }
}