using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI.Crab;

public class CrabBlockAction : EnemyChanceBasedAction
{
    public override Intent Intent { get; } = new("", "res://art/tile_0101.png");
    protected override AudioStream? Sound { get; } = SnekUtility.LoadSound("res://art/enemy_block.ogg");
    public override float ChanceWeight => 1;

    private const int Block = 6;

    public override async Task PerformActionAsync(CancellationToken cancellationToken)
    {
        if (Target == null || Enemy == null)
        {
            GD.Print("target or enemy is null");
            return;
        }

        var blockEffect = new BlockEffect(Block) { Sound = Sound };
        await blockEffect.ExecuteAllAsync([Enemy], cancellationToken);

        await SnekUtility.DelayGd(0.6f, cancellationToken);
    }
}