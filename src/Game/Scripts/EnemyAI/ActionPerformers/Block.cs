using CardGameV1.EffectSystem;

namespace CardGameV1.EnemyAI.ActionPerformers;

public class Block(int blockAmount) : ActionPerformer
{
    private string SoundPath { get; init; } = "res://art/block.ogg";

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        var blockEffect = new BlockEffect(blockAmount) { Sound = SnekUtility.LoadSound(SoundPath) };
        await blockEffect.ExecuteAllAsync([Enemy], cancellationToken);

        await SnekUtility.DelayGd(0.6f, cancellationToken);
    }

    public override string DisplayText => "";
}