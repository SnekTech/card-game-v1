using System.Threading;
using System.Threading.Tasks;
using Godot;
using GTweens.Builders;
using GTweensGodot.Extensions;

namespace CardGameV1;

public static class SnekUtility
{
    public static Task DelayGd(float timeSec)
    {
        return GTweenSequenceBuilder.New().AppendTime(timeSec).Build().PlayAsync(CancellationToken.None);
    }

    public static PackedScene LoadScene(string path) => ResourceLoader.Load<PackedScene>(path);
    public static Texture2D LoadTexture(string path) => ResourceLoader.Load<Texture2D>(path);
}