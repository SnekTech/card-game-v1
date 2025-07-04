using System.Threading;
using CardGameV1.Character;
using CardGameV1.EffectSystem;

namespace CardGameV1.EnemyAI;

public abstract class EnemyAction
{
    public abstract Intent Intent { get; }

    protected virtual AudioStream? Sound => null;

    public Enemy? Enemy { get; set; }
    public ITarget? Target { get; set; }

    public abstract Task PerformActionAsync(CancellationToken cancellationToken);

    public virtual void UpdateIntentText()
    {
        Intent.CurrentText = Intent.BaseText;
    }
}