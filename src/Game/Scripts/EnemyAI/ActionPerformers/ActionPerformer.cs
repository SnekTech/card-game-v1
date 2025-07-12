using CardGameV1.Character;
using CardGameV1.EffectSystem;

namespace CardGameV1.EnemyAI.ActionPerformers;

public abstract class ActionPerformer
{
    public required Enemy Enemy { get; init; }
    public ITarget? Target { get; set; }

    public abstract Task PerformActionAsync(CancellationToken cancellationToken);
    public abstract string DisplayText { get; }
}