using CardGameV1.EffectSystem;

namespace CardGameV1.EnemyAI.Crab;

public class CrabBlockAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new("", "res://art/tile_0101.png");
    public override float ChanceWeight => 1;

    private const int Block = 6;
    private static readonly AudioStream BlockSound = SnekUtility.LoadSound("res://art/enemy_block.ogg");

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block) { Sound = BlockSound };
        await blockEffect.ExecuteAllAsync([Enemy], cancellationToken);

        await SnekUtility.DelayGd(0.6f, cancellationToken);
    }
}