using CardGameV1.EffectSystem;
using CardGameV1.EnemyAI.ActionPerformers;

namespace CardGameV1.EnemyAI;

public abstract class EnemyAction
{
    public required Intent Intent { get; init; }
    public required ActionPerformer ActionPerformer { get; init; }
    public void UpdateTarget(ITarget target) => ActionPerformer.Target = target;

    public Task PerformActionAsync(CancellationToken cancellationToken) =>
        ActionPerformer.PerformActionAsync(cancellationToken);

    // todo: fix dynamic intent text
    public virtual void UpdateIntentText()
    {
        Intent.CurrentText = Intent.BaseText;
    }
}