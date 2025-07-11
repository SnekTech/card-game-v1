namespace CardGameV1.EnemyAI.Crab;

public class CrabMegaBlockAction : EnemyConditionalAction
{
    // todo: fix conditional mega block

    // public override Intent Intent { get; } = new("", "res://art/tile_0102.png");
    //
    // private const int Block = 15;
    // private const int Threshold = 6;
    //
    // private bool _alreadyUsed;
    //
    // public override bool IsPerformable()
    // {
    //     if (Enemy == null || _alreadyUsed)
    //         return false;
    //
    //     var healthIsLowEnough = Enemy.Stats.Health <= Threshold;
    //
    //     _alreadyUsed = healthIsLowEnough;
    //     return healthIsLowEnough;
    // }
    //
    // public async Task PerformActionAsync(CancellationToken cancellationToken)
    // {
    //     if (Enemy == null)
    //     {
    //         GD.Print("target or enemy is null");
    //         return;
    //     }
    //
    //     var blockEffect = new BlockEffect(Block) { Sound = BlockSound };
    //     await blockEffect.ExecuteAllAsync([Enemy], cancellationToken);
    //
    //     await SnekUtility.DelayGd(0.6f, cancellationToken);
    // }
}