using System.Threading;
using System.Threading.Tasks;
using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

namespace CardGameV1;

public static class SnekUtility
{
    public static Task DelayGd(float timeSec, CancellationToken cancellationToken = default)
    {
        return GTweenSequenceBuilder.New().AppendTime(timeSec).Build().PlayAsync(cancellationToken);
    }

    public static PackedScene LoadScene(string path) => ResourceLoader.Load<PackedScene>(path);
    public static Texture2D LoadTexture(string path) => ResourceLoader.Load<Texture2D>(path);
    public static AudioStream LoadSound(string path) => ResourceLoader.Load<AudioStream>(path);
}