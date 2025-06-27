using System.Threading;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using Godot;

namespace CardGameV1.EnemyAI.Crab;

public class CrabMegaBlockAction : EnemyConditionalAction
{
    public override Intent Intent { get; } = new("", "res://art/tile_0102.png");
    protected override AudioStream? Sound { get; } = SnekUtility.LoadSound("res://art/enemy_block.ogg");

    private const int Block = 15;
    private const int Threshold = 6;

    private bool _alreadyUsed;

    public override bool IsPerformable()
    {
        if (Enemy == null || _alreadyUsed)
            return false;

        var healthIsLowEnough = Enemy.Stats.Health <= Threshold;

        _alreadyUsed = healthIsLowEnough;
        return healthIsLowEnough;
    }

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