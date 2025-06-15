using System.Threading;
using System.Threading.Tasks;
using GTweens.Builders;
using GTweensGodot.Extensions;

namespace CardGameV1;

public static class SnekUtility
{
    public static Task DelayGd(float timeSec)
    {
        return GTweenSequenceBuilder.New().AppendTime(timeSec).Build().PlayAsync(CancellationToken.None);
    }
}